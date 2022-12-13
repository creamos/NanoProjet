using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class CallAudioTransition : MonoBehaviour
{
    GameManager _gameManager;
    [SerializeField] AudioManagerSO _audioManagerSO;
    [SerializeField] float _changePhaseTiming;

    [ShowNativeProperty]
    float remainingTime => waiting ? _changePhaseTiming - _gameManager.GameTime : float.NaN;
    bool waiting = false;

    private void Awake ()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start ()
    {
        if (_gameManager) {
            _gameManager.GameStarted.AddListener(WaitForTransition);
        }
    }

    private void WaitForTransition ()
    {
        _gameManager.GameStarted.RemoveListener(WaitForTransition);
        StartCoroutine(CallAudioAtTiming());
    }

    IEnumerator CallAudioAtTiming ()
    {
        waiting = true;
        yield return new WaitUntil(() => _gameManager.GameTime >= _changePhaseTiming);

        _audioManagerSO?.OnPaceChanged();
        waiting = false;
    }
}
