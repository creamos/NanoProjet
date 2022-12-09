using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

[RequireComponent(typeof(PlayerInputManager))]
public class OnPlayerJoinHandler : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    [SerializeField] private PlayersManagerSO _playerManager;

    private void Awake ()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Start ()
    {
        if (_playerInputManager) {
            _playerInputManager.onPlayerJoined -= OnPlayerJoin;
            _playerInputManager.onPlayerJoined += OnPlayerJoin;
        }
    }

    private void OnDestroy ()
    {
        if(_playerInputManager) {
            _playerInputManager.onPlayerJoined -= OnPlayerJoin;
        }
    }

    private void OnPlayerJoin (PlayerInput playerInput)
    {
        Debug.Log("Player Joined");
        _playerManager?.AddPlayer(playerInput);
    }
}