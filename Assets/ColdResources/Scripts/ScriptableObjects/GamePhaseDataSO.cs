using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Phase Data")]
public class GamePhaseDataSO : ScriptableObject
{
    public float startTime = 0;
    public float endTime = 10;

    [ShowNativeProperty]
    public float duration => endTime - startTime;

    public EncounterData[] encounters;
}

[Serializable]
public struct EncounterData
{
    public GameObject encounter;
    [MinMaxSlider(0f,10f)]
    public Vector2 delay;
    
    [MinMaxSlider(0f,1f)]
    public Vector2 rangePosition;
}