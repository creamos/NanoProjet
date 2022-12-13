using System;
using UnityEngine;

[RequireComponent(typeof(TargetGroupProcessor))]
public class TargetGroupFeeder : MonoBehaviour
{
    [SerializeField] private PlayersManagerSO _playerManager;
    TargetGroupProcessor _targetGroupProcessor;

    private void Start ()
    {
        _playerManager.PlayerJoins -= TrackPlayer;
        _playerManager.PlayerLeaves -= UntrackPlayer;
        _playerManager.PlayerJoins += TrackPlayer;
        _playerManager.PlayerLeaves += UntrackPlayer;

        _targetGroupProcessor = GetComponent<TargetGroupProcessor>();
    }

    private void OnDestroy ()
    {
        _playerManager.PlayerJoins -= TrackPlayer;
        _playerManager.PlayerLeaves -= UntrackPlayer;
    }

    private void TrackPlayer (int playerID)
    {
        _targetGroupProcessor.targets.Add(_playerManager.players[playerID].transform);
    }

    private void UntrackPlayer (int playerID)
    {
        _targetGroupProcessor.targets.Remove(_playerManager.players[playerID].transform);
    }
}
