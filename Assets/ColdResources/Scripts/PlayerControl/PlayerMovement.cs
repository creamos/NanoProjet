using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerDataReference))]
[RequireComponent(typeof(StudioEventEmitter))]
public class PlayerMovement : MonoBehaviour
{
    public UnityEvent GrazeEvent, CollisionEvent, BoostEvent;

    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody;

    private PlayerDataReference _data;

    [Header("Bounds")]
    [SerializeField] private BoundsDataSO _boundsData;
    [SerializeField, Range(0.001f, 100.0f)] private float _boundsSmoothing = 10.0f;

    [Header("Movement")]
    [SerializeField] private MovementDataSO _movementData;

    [SerializeField, ReadOnly] private List<Transform> _hitObstacles;
    [SerializeField, ReadOnly] private List<Booster> _hitBoosters;

    [Space]
    [SerializeField] private SpriteRenderer _boostPrompt;

    private Vector2 _pos;
    private Vector2 _vel;
    //Modifiers
    private Coroutine _knockbackProcess;
    private Coroutine _boostProcess;
    public float knockbackAmount { get; private set; } = 0.0f;
    private float _boostAmount = 0.0f;

    private InputAction _moveAction;
    private InputAction _fireAction;

    private StudioEventEmitter _emitter;

    static float[] _positions;
    public static float lowestPosition => _positions?.Min() ?? float.NaN;
    public static float highestPosition => _positions?.Max() ?? float.NaN;


#if UNITY_EDITOR
    [ShowNativeProperty]
    float lowestPos => lowestPosition;
    [ShowNativeProperty]
    float highestPos => highestPosition;
#endif

    private void Awake ()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _data = GetComponent<PlayerDataReference>();
    }

    private void OnDestroy ()
    {
        _fireAction.performed -= OnTrigger;
    }

    public void Init ()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _data = GetComponent<PlayerDataReference>();

        _positions ??= new float[_data.ID + 1];

        if (_data.ID >= _positions.Length) {
            _positions = _positions.Concat(new float[_data.ID + 1 - _positions.Length]).ToArray();
        }
        _positions[_data.ID] = 0;
    }

    private void Start ()
    {
        _emitter = GetComponent<StudioEventEmitter>();
        _moveAction = _playerInput.actions["move"];
        _fireAction = _playerInput.actions["fire"];
        _fireAction.performed -= OnTrigger;
        _fireAction.performed += OnTrigger;
        _pos = transform.position;
    }

    private void Update ()
    {
        Vector2 movementValue = _moveAction.ReadValue<Vector2>();
        if (_movementData) {
            _vel.x = movementValue.x * _movementData.movementSpeed;
            _vel.y = -_movementData.fallingSpeed;

            float add_knockback = _vel.y * knockbackAmount * (_movementData.knockbackMultiplier - 1.0f);
            float add_boost = _vel.y * _boostAmount * (_movementData.boostMultiplier - 1.0f);
            _vel.y += add_boost + add_knockback;
        }
        _pos += _vel * Time.deltaTime;
        _pos.y = Mathf.Min(_pos.y, lowestPosition + _boundsData.boundsHeight);

        if (_boundsData) {
            float clamped_horizontal = Mathf.Clamp(_pos.x, - _boundsData.boundsWidth * 0.5f, _boundsData.boundsWidth * 0.5f);
            _pos.x = Mathf.Lerp(_pos.x, clamped_horizontal, _boundsSmoothing * Time.deltaTime);
        }
        transform.position = _pos;

        _positions[_data.ID] = _pos.y;
    }

    private void FixedUpdate ()
    {
        //_rigidbody.MovePosition(_pos);
    }


    private void OnTrigger (InputAction.CallbackContext obj)
    {
        foreach (var booster in _hitBoosters) {
            if (booster.Boost(_data.ID, _pos.y, out float boostAmount)) {
                _boostPrompt.enabled = false;

                Debug.Log("Boost : " + boostAmount);
                ApplyBoost(boostAmount);

                _emitter.Play();

                BoostEvent?.Invoke();
                break;
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Obstacle") && !_hitObstacles.Contains(other.transform)) {
            _hitObstacles.Add(other.transform);
            ApplyKnockback();
            CollisionEvent?.Invoke();
        }
        if (other.TryGetComponent(out Booster booster) && !_hitBoosters.Contains(booster)) {
            _boostPrompt.enabled = true;
            _hitBoosters.Add(booster);
        }
        if (other.CompareTag("Player") && _data.playerData && _data.ID == 0) {
            GrazeEvent?.Invoke();
        }
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Obstacle") && _hitObstacles.Contains(other.transform)) {
            _hitObstacles.Remove(other.transform);
        }
        if (other.TryGetComponent(out Booster booster) && _hitBoosters.Contains(booster)) {
            _boostPrompt.enabled = false;
            _hitBoosters.Remove(booster);
        }
    }

    private void ApplyKnockback ()
    {
        if (_knockbackProcess != null) StopCoroutine(_knockbackProcess);
        _knockbackProcess = StartCoroutine(KnockbackProcess(1.0f));
    }

    private void ApplyBoost (float boostAmount)
    {
        if (_boostProcess != null) StopCoroutine(_boostProcess);
        _boostProcess = StartCoroutine(BoostProcess(boostAmount));
    }

    IEnumerator KnockbackProcess (float amount = 1.0f)
    {
        float knockbackTime = _movementData.knockbackTime;
        while (knockbackTime > 0.0f) {
            float knockbackTime_amount = knockbackTime / _movementData.knockbackTime;
            knockbackAmount = _movementData.knockbackProfile.Evaluate(knockbackTime_amount);

            knockbackTime -= Time.deltaTime;
            yield return null;

            Debug.Log(knockbackAmount);
        }
        knockbackAmount = _movementData.knockbackProfile.Evaluate(0.0f);
        Debug.Log("Knockback ended");
    }

    IEnumerator BoostProcess (float amount = 1.0f)
    {
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
