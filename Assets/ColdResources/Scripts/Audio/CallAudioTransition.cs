using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class CallAudioTransition : MonoBehaviour
{
    GameManager _gameManager;
    [SerializeField] AudioManagerSO _audioManagerSO;
    [SerializeField] float _changePhaseTiming;

#if UNITY_EDITOR
    [SerializeField, NaughtyAttributes.ReadOnly]
    float remainingTime;
    bool waiting = false;
#endif

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

#if UNITY_EDITOR
    private void Update ()
    {
        remainingTime = waiting ? _changePhaseTiming -_gameManager.GameTime : float.NaN;
    }
#endif

    private void WaitForTransition ()
    {
        _gameManager.GameStarted.RemoveListener(WaitForTransition);
        StartCoroutine(CallAudioAtTiming());
    }

    IEnumerator CallAudioAtTiming ()
    {
#if UNITY_EDITOR
        waiting = true;
#endif

        yield return new WaitUntil(() => _gameManager.GameTime >= _changePhaseTiming);
        _audioManagerSO?.OnPaceChanged();

#if UNITY_EDITOR
        waiting = false;
#endif
    }
}
