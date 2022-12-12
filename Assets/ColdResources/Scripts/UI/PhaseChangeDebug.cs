using System;
using UnityEngine;
using UnityEngine.UI;

public class PhaseChangeDebug : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private Text currentPhase, currentTime, remainingTime;

    private void Start ()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null) {
            Destroy(gameObject);
            return;
        }
        _gameManager.PhaseStarted.AddListener(OnPhaseStarted);
        _gameManager.PhaseEnded.AddListener(OnPhaseEnded);
    }
    private void OnPhaseStarted (GamePhaseDataSO phase)
    {
        if (currentPhase) currentPhase.text = $"Phase {_gameManager.currentPhaseID}";
    }

    private void OnPhaseEnded (GamePhaseDataSO arg0)
    {
        if (currentPhase) currentPhase.text = $"Interlude {_gameManager.currentPhaseID} => {_gameManager.currentPhaseID + 1}";
    }

    private void Update ()
    {
        if (_gameManager.isGameRunning == false) return;

        var phase = _gameManager.CurrentPhase;

        if (currentTime) currentTime.text = $"Time : { Time.time - _gameManager.startTime }";

        if (phase == null) {
            if (remainingTime) remainingTime.text = $"Next Phase in : {_gameManager.TimeBeforeNextPhase}";
        } else {
            if (remainingTime) remainingTime.text = $"Phase End in : {phase.duration - _gameManager.TimeInPhase }";
        }

    }
}
