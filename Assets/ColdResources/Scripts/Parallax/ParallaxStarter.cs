using NaughtyAttributes;
using UnityEngine;

public class ParallaxStarter : MonoBehaviour
{
    public bool asStartTime = true;
    [ShowIf("asStartTime")]
    [Min(0)] public float startTime;

    public float startOffset;

    public bool asEndTime;
    [ShowIf("asEndTime")]
    [Min(0)] public float endTime;
}
