using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class PlayerSpeedAudio : MonoBehaviour
{
    StudioEventEmitter _emitter;
    [SerializeField] PlayerMovement _playerMovement;

    private void Awake ()
    {
        _emitter = GetComponent<StudioEventEmitter>();
    }

    private void Update ()
    {
        var speed = (_playerMovement && _playerMovement.isActiveAndEnabled) ? (1 - _playerMovement.knockbackAmount) : 0;
        _emitter.SetParameter("PlayerSpeed", speed);
    }

}
