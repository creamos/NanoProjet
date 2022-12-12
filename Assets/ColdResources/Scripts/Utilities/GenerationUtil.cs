using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GenerationUtil : MonoBehaviour
{
    [SerializeField] private ObjectsGenerator generator;
    
    [Button]
    private void StartGenerator() {
        if (generator != null) generator.StartGenerating();
    }
    [Button]
    private void StopGenerator() {
        if (generator != null) generator.StopGenerating();
    }
}
