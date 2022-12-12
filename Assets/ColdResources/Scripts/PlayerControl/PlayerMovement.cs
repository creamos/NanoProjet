using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{    
    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody;

    [Header("Bounds")]
    [SerializeField] private BoundsDataSO _boundsData;
    [SerializeField, Range(0.001f, 100.0f)] private float _boundsSmoothing = 10.0f;

    [Header("Movement")]
    [SerializeField] private MovementDataSO _movementData;

    [SerializeField, ReadOnly] private List<Transform> _hitObstacles; 
    [SerializeField, ReadOnly] private List<Transform> _hitBoosters; 

    private Vector2 _pos;
    private Vector2 _vel;
    //Modifiers
    private Coroutine _knockbackProcess;
    private Coroutine _boostProcess;
    private float _knockbackAmount = 0.0f;
    private float _boostAmount = 0.0f;

    private InputAction _moveAction;
    private InputAction _fireAction;

    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _moveAction = _playerInput.actions["move"];
        _fireAction = _playerInput.actions["fire"];
        enabled = false;
    }

    private void Update() {
        Vector2 movementValue = _moveAction.ReadValue<Vector2>();
        if (_movementData) {
            _vel.x = movementValue.x * _movementData.movementSpeed;
            _vel.y = -_movementData.fallingSpeed;
        
            float add_knockback = _vel.y * _knockbackAmount * (_movementData.knockbackMultiplier - 1.0f);
            float add_boost = _vel.y * _boostAmount * (_movementData.boostMultiplier - 1.0f);
            _vel.y += add_boost + add_knockback;
        }
        _pos = _pos + _vel * Time.deltaTime;

        if (_boundsData) {
            float clamped_horizontal = Mathf.Clamp(_pos.x, -_boundsData.boundsWidth * 0.5f, _boundsData.boundsWidth * 0.5f);
            _pos.x = Mathf.Lerp(_pos.x, clamped_horizontal, _boundsSmoothing * Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        _rigidbody.MovePosition(_pos);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Obstacle") && !_hitObstacles.Contains(other.transform)) {
            _hitObstacles.Add(other.transform);
            ApplyKnockback();
        }
        if (other.CompareTag("Booster") && !_hitBoosters.Contains(other.transform)) {
            _hitBoosters.Add(other.transform);
            ApplyBoost();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Obstacle") && _hitObstacles.Contains(other.transform)) {
            _hitObstacles.Remove(other.transform);
        }
        if (other.CompareTag("Booster") && _hitBoosters.Contains(other.transform)) {
            _hitBoosters.Remove(other.transform);
        }
    }

    private void ApplyKnockback() {
        if (_knockbackProcess != null) StopCoroutine(_knockbackProcess);
        _knockbackProcess = StartCoroutine("KnockbackProcess", 1.0f);
    }

    private void ApplyBoost() {
        if (_boostProcess != null) StopCoroutine(_boostProcess);
        _boostProcess = StartCoroutine("BoostProcess", 1.0f);
    }

    IEnumerator KnockbackProcess(float amount = 1.0f) {
        float knockbackTime = _movementData.knockbackTime;
        while (knockbackTime > 0.0f) {
            float knockbackTime_amount = knockbackTime / _movementData.knockbackTime;
            _knockbackAmount = _movementData.knockbackProfile.Evaluate(knockbackTime_amount);

            knockbackTime -= Time.deltaTime;
            yield return null;

            Debug.Log(_knockbackAmount);
        }
        _knockbackAmount = _movementData.knockbackProfile.Evaluate(0.0f);
        Debug.Log("Knockback ended");
    }

    IEnumerator BoostProcess(float amount = 1.0f) {
        float boostTime = _movementData.boostTime * amount;
        while (boostTime > 0.0f) {
            float boostTime_amount = boostTime / _movementData.boostTime;
            _boostAmount = _movementData.boostProfile.Evaluate(boostTime_amount);

            boostTime -= Time.deltaTime;
            yield return null;
        }
        _boostAmount = _movementData.boostProfile.Evaluate(0.0f);
    }
}
