using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Manager")]
public class PlayersManagerSO : ScriptableObject
{
    public event Action<int> PlayerJoins;
    public event Action<int> PlayerLeaves;

    public PlayerDataSO _player1Data, _player2Data;


    [ReadOnly]
    public PlayerInput[] _players = new PlayerInput[2];
    [ShowNativeProperty] public int nbPlayers => _players.Count(p => p != null);



    public PlayerInput this[int key] {
        get {
            if (key < 0 || key >= _players.Length)
                return null;
            else
                return _players[key];
        }
    }

    private void Awake ()
    {
        _players = new PlayerInput[2] { null, null };
    }

    public void AddPlayer (PlayerInput playerInput)
    {
        int joiningPlayerID = -1;
        if (_players[0] == null) {
            joiningPlayerID = 0;
        } else if (_players[1] == null) {
            joiningPlayerID = 1;
        } else {
            // TODO : Replace keyboard user if this one is a gamepad
        }

        if (joiningPlayerID != -1) {
            _players[joiningPlayerID] = playerInput;
            InitPlayer(joiningPlayerID, playerInput.gameObject);
            PlayerJoins?.Invoke(joiningPlayerID);
        } else {
            Destroy(playerInput.gameObject);
        }
    }

    private void InitPlayer (int id, GameObject player)
    {
        switch (id) {
        case 0: player.GetComponent<PlayerSetup>().Init(_player1Data); break;
        case 1: player.GetComponent<PlayerSetup>().Init(_player2Data); break;
        }
    }

    public void RemovePlayer (PlayerInput playerInput)
    {
        if (playerInput == null) return;

        int leavingPlayerID;
        if (_players[0] == playerInput) {
            leavingPlayerID = 0;
        } else if (_players[1] == playerInput) {
            leavingPlayerID = 1;
        } else return;

        PlayerLeaves?.Invoke(leavingPlayerID);
    }

}
