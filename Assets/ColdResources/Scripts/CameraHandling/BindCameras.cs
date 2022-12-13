using UnityEngine;

public class BindCameras : MonoBehaviour
{
    [SerializeField] Camera _gameplayCamera;

    private void LateUpdate ()
    {
        transform.position = new Vector3(transform.position.x, _gameplayCamera.transform.position.y, transform.position.z);
    }
}
