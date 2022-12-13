using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private PlayersManagerSO _playerManager;
    public UnityEvent GameStarted;

#if UNITY_EDITOR
    [SerializeField] bool _debugSoloMode = false;
#endif
    public bool[] playersReady { get; private set; } = { false, false };


#if UNITY_EDITOR
    [ShowNativeProperty()]
    public bool canStart => (_debugSoloMode && playersReady.Contains(true)) || !playersReady.Contains(false);
#else
    [ShowNativeProperty()]
    public bool canStart => !playersReady.Contains(false);
#endif

    private void Awake ()
    {
        if (_playerManager) {
            _playerManager.PlayerLeaves += OnPlayerLeave;
            _playerManager.PlayerJoins += OnPlayerJoins;
        }
        GameStarted.AddListener(OnGameStarted);
    }

    private void OnPlayerJoins(int playerID)
    {
        var readyAction = _playerManager[playerID].actions.FindAction("Fire");


        if(playerID == 0) readyAction.performed += OnPlayer1Ready;
        if(playerID == 1) readyAction.performed += OnPlayer2Ready;
    }

    private void OnPlayerLeave (int playerID)
    {
        playersReady = new bool[]{ false, false };
    }

    private void OnPlayerReady (int playerID)
    {
        Debug.Log($"Player {playerID + 1} Ready");
        if(_playerManager.nbPlayers == 2 || _debugSoloMode) {
            playersReady[playerID] = true;
            if (canStart) {
                Debug.Log("GameStart");
                GameStarted?.Invoke();
            }
        }
    }

    private void OnPlayer1Ready(InputAction.CallbackContext _) => OnPlayerReady(0);
    private void OnPlayer2Ready(InputAction.CallbackContext _) => OnPlayerReady(1);

    private void OnGameStarted() {
        PlayerMovement movement;
        foreach (PlayerInput player in _playerManager.players)
        {
            if(!player) continue;


            var readyAction = player.actions.FindAction("Fire");


            readyAction.performed -= OnPlayer1Ready;
            readyAction.performed -= OnPlayer2Ready;

            movement = player.GetComponent<PlayerMovement>();
            movement.enabled = true;
        }
    }
}
