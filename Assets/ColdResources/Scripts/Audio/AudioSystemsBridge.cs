using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
public class AudioSystemsBridge : MonoBehaviour
{
    [SerializeField] AudioManagerSO _managerSO;
    AudioManager _manager;

    private void Awake ()
    {
        _manager = GetComponent<AudioManager>();
    }

    private void Start ()
    {
        _managerSO.RunStarted.AddListener(_manager.StartMusic);
        _managerSO.PlayerObstacleCollision.AddListener(_manager.PlaySlowSound);
        //_managerSO.PlayerBoost.AddListener(_manager.PlayBoostSound);
        _managerSO.PaceChanged.AddListener(_manager.ChangeMusicToHardmode);
    }

    private void OnDestroy ()
    {
        _managerSO.RunStarted.RemoveListener(_manager.StartMusic);
        _managerSO.PlayerObstacleCollision.RemoveListener(_manager.PlaySlowSound);
        //_managerSO.PlayerBoost.RemoveListener(_manager.PlayBoostSound);
        _managerSO.PaceChanged.RemoveListener(_manager.ChangeMusicToHardmode);
    }
}
