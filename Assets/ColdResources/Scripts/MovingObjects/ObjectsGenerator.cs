using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField] private BoundsDataSO _boundsData;

    [Header("Phase Generation")]
    [SerializeField] private GamePhaseDataSO _phaseData;
    [SerializeField, ReadOnly] private List<Transform> _generatedObjects = new List<Transform>();
    [SerializeField, ReadOnly] private List<Bounds> _generatedObjectsBounds = new List<Bounds>();
    private Coroutine _generation;

    [Button]
    public void StartGenerating() {
        Debug.Log("Started generation");
        if (_generation != null) StopCoroutine(_generation);
        _generation = StartCoroutine("ObjectsSpawnLoop");
    }

    [Button]
    public void StopGenerating() {
        Debug.Log("Stopped generation");
        if (_generation != null) StopCoroutine(_generation);
    }

    private void Update() {
        Vector2 destroy_pos = GetDestroyPosition();

        int i = 0;
        Transform generated_object;
        Bounds generated_bounds;
        while (i < _generatedObjects.Count) {
            generated_object = _generatedObjects[i];
            generated_bounds = _generatedObjectsBounds[i];
            if (generated_bounds.center.y > destroy_pos.y && !generated_bounds.Contains(destroy_pos)) {
                Destroy(generated_object.gameObject);
                _generatedObjects.RemoveAt(i);
                _generatedObjectsBounds.RemoveAt(i);
            }
            else i++;
        }
    }

    IEnumerator ObjectsSpawnLoop() {
        while (true) {
            float wait_time = 1.0f;

            // Only process if there is phase data
            if (_phaseData && _phaseData.encounters.Length > 0) {
                EncounterData encounter_data = GetRandomEncounter();
                Vector2 generated_pos = GetSpawnPosition(encounter_data);
                GameObject generated_object = Instantiate(encounter_data.encounter, generated_pos, Quaternion.identity);
                Bounds generated_bounds = GetObjectBounds(generated_object);
                
                wait_time = Random.Range(encounter_data.delay.x, encounter_data.delay.y);
                
                _generatedObjects.Add(generated_object.transform);
                _generatedObjectsBounds.Add(generated_bounds);
            }
            yield return new WaitForSeconds(wait_time);
        }
    }

    private EncounterData GetRandomEncounter() {
        // For each object, add `weight` times the object's id for weighting
        int chosen_id = 0; // By default, chosen encounter is the 1st one
        List<int> weighted_ids = new List<int>();

        for (int i = 0; i < _phaseData.encounters.Length; i++)
        {
            weighted_ids.AddRange(Enumerable.Repeat<int>(i, _phaseData.encounters[i].weight));
        }
        // Count will be 0 if every encounter is weighted at 0
        if (weighted_ids.Count > 0) chosen_id = weighted_ids[Random.Range(0, weighted_ids.Count - 1)];
        
        return _phaseData.encounters[chosen_id];
    }

    private Vector2 GetSpawnPosition(EncounterData encounter) {
        float horizontal_pos = Random.Range(encounter.rangePosition.x, encounter.rangePosition.y) - 0.5f;
        horizontal_pos *= _boundsData.boundsWidth + _boundsData.padding.x;

        Vector2 target_pos = Vector2.zero;
        target_pos.y = PlayerMovement.lowestPosition;
        target_pos.x = horizontal_pos;
        target_pos.y -= _boundsData.boundsHeight + _boundsData.padding.y;
        
        return target_pos;
    }

    private Bounds GetObjectBounds(GameObject generated_object) {
        Renderer[] renderers = generated_object.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(generated_object.transform.position, Vector3.zero);
        Bounds b = renderers[0].bounds;
        foreach (Renderer r in renderers) {
            b.Encapsulate(r.bounds);
        }
        Debug.Log(b);
        return b;
    }

    private Vector2 GetDestroyPosition() {
        Vector2 target_pos = Vector2.zero;
        target_pos.y = PlayerMovement.highestPosition;
        target_pos.x = 0.0f;
        target_pos.y += _boundsData.boundsHeight + _boundsData.padding.y;
        
        return target_pos;
    }

    public void OnStartPhase(GamePhaseDataSO phase) {
        _phaseData = phase;
        StartGenerating();
    }

    public void OnEndPhase(GamePhaseDataSO phase) {
        if (phase = _phaseData) {
            StopGenerating();
        } 
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        foreach (Bounds generated_bounds in _generatedObjectsBounds)
        {
            Gizmos.DrawWireCube(generated_bounds.center, generated_bounds.size);
        }
    }
}
