using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Bounds Data")]
public class BoundsDataSO : ScriptableObject
{
    [Range(1, 100)] public float boundsHeight = 20.0f;
    [Range(1, 100)] public float boundsWidth = 10.0f;
    public Vector2 padding = Vector2.one;


    public float GetRandomHorizontal() {
        return Random.Range(
            -boundsWidth * 0.5f - padding.x,
            boundsWidth * 0.5f + padding.x 
        );
    }
    public float GetRandomVertical() {
        return Random.Range(
            -boundsHeight * 0.5f - padding.y,
            boundsHeight * 0.5f + padding.y 
        );
    }
}
