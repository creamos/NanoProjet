using Cinemachine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class TargetGroupProcessor : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();
    CinemachineTargetGroup _targetGroup;

    Transform _bait;

    private void Awake ()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start ()
    {
        var transposer = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_AdjustmentMode = CinemachineFramingTransposer.AdjustmentMode.ZoomOnly;

        _bait = new GameObject("Camera Computed Target").transform;
        _bait.parent = transform.parent;

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
        var targetByDst = targets.Select(t=>t.position.y).OrderBy(height => height);
        switch(targetByDst.Count()) {
        case 2:
            var lowest = targetByDst.First();
            var highest = targetByDst.Last();
            var baitPos = (lowest + highest) / 2f;
            _bait.position = Vector3.up * baitPos;

            _targetGroup.m_Targets[0].radius = (highest - lowest)/2f + 1f;
            break;
        case 1:
            var height = targetByDst.First();
            _bait.position = Vector3.up * height;
            _targetGroup.m_Targets[0].radius = 1f;
            break;
        case 0:
            _bait.position = Vector3.zero;
            _targetGroup.m_Targets[0].radius = 1f;
            break;
        }
    }
}
