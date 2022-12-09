using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJoiningUI : MonoBehaviour
{
    [SerializeField]
    private PlayersManagerSO _playersManagerSO;
    private GameStarter _gameStarter;

    [SerializeField] Toggle _toggleP1, _toggleP2;
    [SerializeField] Text _labelP1, _labelP2;


    private void Awake ()
    {
        _gameStarter = FindObjectOfType<GameStarter>();
        if (_gameStarter == null) gameObject.SetActive(false);
    }

    private void Update ()
    {
        _toggleP1.isOn = _gameStarter.playersReady[0];
        _toggleP2.isOn = _gameStarter.playersReady[1];

        string p1text = _playersManagerSO._players[0] != null ? (_gameStarter.playersReady[0] ? "Ready" : "Not Ready") : "None";
        string p2text = _playersManagerSO._players[1] != null ? (_gameStarter.playersReady[1] ? "Ready" : "Not Ready") : "None";

        _labelP1.text = $"Player 1 : {p1text}";
        _labelP2.text = $"Player 2 : {p2text}";
    }
}
