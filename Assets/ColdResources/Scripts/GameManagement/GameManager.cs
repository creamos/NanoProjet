using NaughtyAttributes;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    [SerializeField] PlayersManagerSO _playerManagerSO;

    public UnityEvent<GamePhaseDataSO> PhaseStarted, PhaseEnded;

    [SerializeField] GamePhaseDataSO[] _phases;

    [field: ShowNonSerializedField]
    public int currentPhaseID { get; private set; }

    public GamePhaseDataSO CurrentPhase => waitForNextPhase || currentPhaseID >= _phases.Length? null : _phases[currentPhaseID];
    public bool waitForNextPhase { get; private set; } = false;

    public float TimeInPhase => waitForNextPhase ? 0f : Time.time - _currentPhaseStartTime;
    public float TimeBeforeNextPhase => waitForNextPhase? _currentPhaseStartTime - Time.time : 0f;
    
    public bool isGameRunning { get; private set; } = false;
    public float startTime { get; private set; }


    float _currentPhaseStartTime => startTime + _phases[currentPhaseID].startTime;
    float _currentPhaseEndTime => _currentPhaseStartTime + _phases[currentPhaseID].duration;

    [Button("Load Game Phases")]
    void LoadGamePhases ()
    {
        _phases = Resources.LoadAll<GamePhaseDataSO>("ScriptableObjects/GamePhases");
        ReorderPhases();
    }

    [Button("Reorder Phases List")]
    void ReorderPhases ()
    {
        _phases = _phases.OrderBy(phase => phase.startTime).ToArray();
    }

    public void OnGameStart ()
    {
        isGameRunning = true;
        StartCoroutine(PhaseChangeRoutine());
    }

    IEnumerator PhaseChangeRoutine ()
    {
        startTime = Time.time;
        currentPhaseID = 0;


        do {
            waitForNextPhase = true;

            yield return new WaitUntil(() => Time.time >= _currentPhaseStartTime);

            waitForNextPhase = false;

            PhaseStarted?.Invoke(CurrentPhase);

            yield return new WaitUntil(() => Time.time >= _currentPhaseEndTime);

            PhaseEnded?.Invoke(CurrentPhase);
            currentPhaseID++;

        } while (currentPhaseID < _phases.Length);
    }
}
