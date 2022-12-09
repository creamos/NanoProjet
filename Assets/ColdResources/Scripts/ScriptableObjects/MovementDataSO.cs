using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Movement Data")]
public class MovementDataSO : ScriptableObject
{
    [Range(0.0f, 100.0f)] public float fallingSpeed = 10.0f; // m/s
    [Range(0.0f, 100.0f)] public float movementSpeed = 10.0f; // m/s

    [Space]
    [Range(0.0f, 1.0f)] public float knockbackMultiplier = 0.8f;
    [Range(1.0f, 2.0f)] public float boostMultiplier = 1.5f;

    [Space]
    public float knockbackTime = 0.5f;
    public float boostTime = 0.5f;

    [Space]
    [CurveRange(0.0f, 0.0f, 1.0f, 1.0f, EColor.Red)] public AnimationCurve knockbackProfile;
    [CurveRange(0.0f, 0.0f, 1.0f, 1.0f, EColor.Blue)] public AnimationCurve boostProfile;


}
