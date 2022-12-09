using System.Collections;
using System.Collections.Generic;
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
    [SerializeField, Range(0.001f, 100.0f)] private float _accelerationTime = 0.5f;
    [SerializeField, Range(0.001f, 100.0f)] private float _movementSpeed = 10.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float _movementAmount = 1.0f;
    [SerializeField] private AnimationCurve _movementAmountCurve;

    private Vector2 _pos;

    private InputAction _moveAction;
    private InputAction _fireAction;

    private Coroutine _knockbackCoroutine;

    
    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _moveAction = _playerInput.actions["move"];
        _fireAction = _playerInput.actions["fire"];
    }

    private void Update() {
        Vector2 movementValue = _moveAction.ReadValue<Vector2>();
        _pos.x += movementValue.x * _movementSpeed * Time.deltaTime;

        if (_boundsData) {
            Vector2 clamped_pos = new Vector2(
                Mathf.Clamp(
                    _pos.x, 
                    _boundsData.boundsWidth * -0.5f, 
                    _boundsData.boundsWidth * 0.5f
                ),
                Mathf.Clamp(
                    _pos.y, 
                    _boundsData.boundsHeight * -0.5f, 
                    _boundsData.boundsHeight * 0.5f
                )
            );
            _pos = Vector2.Lerp(_pos, clamped_pos, _boundsSmoothing * Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        _rigidbody.MovePosition(_pos);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.ToString());
        if (other.CompareTag("Obstacle")) {
            if (_knockbackCoroutine != null) StopCoroutine(_knockbackCoroutine);
            _knockbackCoroutine = StartCoroutine("HitKnockback");
        }
    }

    IEnumerator HitKnockback() {
        float time = 0.0f;
        while (time <= 1.0f) {

            yield return null;
        }
    }
}