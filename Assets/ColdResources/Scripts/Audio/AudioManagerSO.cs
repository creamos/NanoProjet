using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio Manager")]
public class AudioManagerSO : ScriptableObject
{
    #region Events

    // GAME STATES RELATED

    [Foldout("Events")] [Tooltip("Event called when the game scene is loaded, the run isn't started yet.")]
    public UnityEvent GameLoaded;

    [Foldout("Events")] [Tooltip("Event called when all players are ready and the run starts.")]
    public UnityEvent RunStarted;

    [Foldout("Events")] [Tooltip("Event called when the players finish the run.")]
    public UnityEvent RunFinished;

    // PLAYER ACTIONS RELATED

    [Tooltip("Event called when the two players collide with each other.")]
    [Foldout("Events")] public UnityEvent PlayersGraze;

    [Tooltip("Event called when a player interact with a booster.")]
    [Foldout("Events")] public UnityEvent PlayerBoost;

    [Tooltip("Event called when a player hit an obstacle.")]
    [Foldout("Events")] public UnityEvent PlayerObstacleCollision;

    #endregion

    #region Audio Events

    [Foldout("FMOD Events")] public EventReference AudioEvent_GameLoaded;
    [Foldout("FMOD Events")] public EventReference AudioEvent_RunStarted;
    [Foldout("FMOD Events")] public EventReference AudioEvent_RunFinished;

    [Foldout("FMOD Events")] public EventReference AudioEvent_PlayersGraze;
    [Foldout("FMOD Events")] public EventReference AudioEvent_PlayerObstacleCollision;
    [Foldout("FMOD Events")] public EventReference AudioEvent_PlayerBoost;

    #endregion

    #region Methods

    /// <summary>
    /// Called this method to notify the audiomanager that the game is loaded.
    /// </summary>
    public void OnGameLoaded ()
    {
        GameLoaded?.Invoke();
        Debug.Log("[AudioManager] Game Loaded");
        if (!AudioEvent_GameLoaded.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(AudioEvent_GameLoaded);

    }

    /// <summary>
    /// Called this method to notify the audiomanager that the run is started.
    /// </summary>
    public void OnRunStarted ()
    {
        RunStarted?.Invoke();
        Debug.Log("[AudioManager] Run Started");
        if (!AudioEvent_RunStarted.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(AudioEvent_RunStarted);


    }

    /// <summary>
    /// Called this method to notify the audiomanager that the run is finished.
    /// </summary>
    public void OnRunFinished ()
    {
        RunFinished?.Invoke();
        Debug.Log("[AudioManager] Run Finised");
        if (!AudioEvent_RunFinished.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(AudioEvent_RunFinished);


    }

    /// <summary>
    /// Called this method to notify the audiomanager that the players just grazed.
    /// </summary>
    public void OnPlayersGraze ()
    {
        PlayersGraze?.Invoke();
        Debug.Log("[AudioManager] Players Graze");
        if (!AudioEvent_PlayersGraze.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(AudioEvent_PlayersGraze);


    }

    /// <summary>
    /// Called this method to notify the audiomanager that a player has collided with an obstacle.
    /// </summary>
    public void OnPlayerObstacleCollision ()
    {
        PlayerObstacleCollision?.Invoke();
        Debug.Log("[AudioManager] Player Collided");
        if (!AudioEvent_PlayerObstacleCollision.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(AudioEvent_PlayerObstacleCollision);


    }

    /// <summary>
    /// Called this method to notify the audiomanager that a player has triggered a boost.
    /// </summary>
    public void OnPlayersBoost ()
    {
        PlayerBoost?.Invoke();
        Debug.Log("[AudioManager] Player Boosted");
        if(!AudioEvent_PlayerBoost.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(AudioEvent_PlayerBoost);


    }

    #endregion
}
