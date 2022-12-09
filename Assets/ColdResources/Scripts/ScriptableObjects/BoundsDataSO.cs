using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Bounds Data")]
public class BoundsDataSO : ScriptableObject
{
    [SerializeField, Range(1, 100)] public float boundsHeight = 20.0f;
    [SerializeField, Range(1, 100)] public float boundsWidth = 10.0f;
}
