using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MovingObjectsManager : MonoBehaviour
{
    [Header("Bounds")]
    [SerializeField] private BoundsDataSO _boundsData;

    [Header("Object Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector2 _moveDirection = Vector2.up;
    
    [Header("Object Management")]
    [SerializeField] private bool _generateObjects = true;
    private Coroutine _generation;
    [SerializeField, MinMaxSlider(0.0f, 10.0f)] private Vector2 _objectSpawningDelay = new Vector2(1.0f, 5.0f);
    [SerializeField] private List<Transform> _objectPrefabs;
    [SerializeField, ReadOnly] private List<Transform> _managedObjects;

    public void StartGenerating() {
        Debug.Log("Started generation");
        _generateObjects = true;
        _generation = StartCoroutine("ObjectsSpawnLoop");
    }

    public void StopGenerating() {
        Debug.Log("Stopped generation");
        _generateObjects = false;
        StopCoroutine(_generation);
    }

    private void Update() {
        Transform movingObject;
        int i = 0;
        while (i < _managedObjects.Count) {
            movingObject = _managedObjects[i];
            movingObject.Translate(_moveDirection * _moveSpeed * Time.deltaTime);

            if (movingObject.position.y > (_boundsData.boundsHeight + _boundsData.padding.y)) {
                Debug.Log(string.Format("Object {0} was too high ({1}) and was deleted", movingObject, movingObject.position.y));
                _managedObjects.Remove(movingObject);
                Destroy(movingObject.gameObject);
            }
            else i++;
        }
    }

    IEnumerator ObjectsSpawnLoop() {
        while (_generateObjects) {
            if (_objectPrefabs.Count > 0) {
                Vector3 start_pos = Vector3.zero;
                start_pos.x = _boundsData.GetRandomHorizontal();
                start_pos.y = -(_boundsData.boundsHeight + _boundsData.padding.y);
                int start_id = Random.Range(0, _objectPrefabs.Count - 1);

                Transform new_movingObject = Instantiate<Transform>(_objectPrefabs[start_id], start_pos, Quaternion.identity, transform);
                _managedObjects.Add(new_movingObject);
                Debug.Log(string.Format("Generated {0} at {1}", new_movingObject, start_pos));
            }
            yield return new WaitForSeconds(Random.Range(_objectSpawningDelay.x, _objectSpawningDelay.y));
        }
    }
}
