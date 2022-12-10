using Cinemachine;
using System.Linq;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class TargetGroupProcessor : MonoBehaviour
{
    [SerializeField] Transform[] _targets;
    CinemachineTargetGroup _targetGroup;

    Transform _bait;

    private void Awake ()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();

        var transposer = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_AdjustmentMode = CinemachineFramingTransposer.AdjustmentMode.ZoomOnly;
    }

    private void Start ()
    {
        _bait = new GameObject("Camera Computed Target").transform;

        _targetGroup.m_Targets = new Target[] {
            new Target {
                target = _bait,
                radius = 1f,
                weight = 1f
            }
        };


    }

    private void Update ()
    {
        var targetByDst = _targets.Select(t=>t.position.y).OrderBy(height => height);

        var lowest = targetByDst.First();
        var highest = targetByDst.Last();

        _bait.position = Vector3.up * (lowest + highest) / 2f;

        _targetGroup.m_Targets[0].radius = (highest - lowest)/2f + 1f;
    }
}
