using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private PlayersManagerSO _playerManager;
    public UnityEvent GameStarted;


    public bool[] playersReady { get; private set; } = { false, false };

    [ShowNativeProperty()]
    public bool canStart => !playersReady.Contains(false);

    private void Awake ()
    {
        if (_playerManager) {
            _playerManager.PlayerLeaves += OnPlayerLeave;
            _playerManager.PlayerJoins += OnPlayerJoins;
        }
    }

    private void OnPlayerJoins(int playerID)
    {
        var readyAction = _playerManager[playerID].actions.FindAction("Fire");


        readyAction.performed += (_) => OnPlayerReady(playerID);
    }

    private void OnPlayerLeave (int playerID)
    {
        playersReady = new bool[]{ false, false };
    }

    private void OnPlayerReady (int playerID)
    {
        Debug.Log($"Player {playerID + 1} Ready");
        if(_playerManager.nbPlayers == 2) {
            playersReady[playerID] = true;
            if (canStart) {
                Debug.Log("GameStart");
                GameStarted?.Invoke();
            }
        }
    }
}
