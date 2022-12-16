using NaughtyAttributes;
using UnityEngine;

[SelectionBase]
public class ParallaxPaner : MonoBehaviour
{
    Camera _cam;

    private float _startPos;
    private float _camOriginAtSpawn;

    public float parallaxAmount;
    public float length;

    public bool isArriving = true;
    public bool isLooping = true;
    public bool isLeaving = false;
    
    private void Start ()
    {
        _cam = GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Camera>();

        _startPos = transform.position.y;
        _camOriginAtSpawn = _cam.transform.position.y;
    }


    private void LateUpdate ()
    {
        Parallax();
    }

    void Parallax ()
    {
        float cameraWidth = _cam.orthographicSize;

        float up_leaveHeight = _cam.transform.position.y + (cameraWidth + length / 2f);
        float down_leaveHeight = _cam.transform.position.y - (cameraWidth + length / 2f);
        //float up_respawnHeight = _cam.transform.position.y - (length / 2f - cameraWidth);
        //float down_respawnHeight = _cam.transform.position.y + (length / 2f - cameraWidth);

        var cameraMovement = _cam.transform.position.y - _camOriginAtSpawn;
        var delta = cameraMovement * parallaxAmount;

        var pos = _startPos + delta;

        // Loop the plane if it goes out of range
        if (isLooping && !(isArriving||isLeaving)) {
            if (pos >= up_leaveHeight ) {
                pos -= length;
                _startPos -= length;
            } else if (pos <= down_leaveHeight) {
                pos += length;
                _startPos += length;
            }
        }

        // If the plane is arrived in range, disable the flag
        if(isArriving && (pos > down_leaveHeight || pos < up_leaveHeight)) {
            isArriving = false;
        }

        // If the plane leaved the range, remove the plane
        if (isLeaving && (pos < down_leaveHeight - length || pos > up_leaveHeight + length)) {
            Destroy(gameObject);
        }

        transform.position = pos * Vector3.up;
    }

    [Button]
    void AutoLength ()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }


    /*
    private void OnDrawGizmosSelected ()
    {
        Vector3 start = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            start + Vector3.up * length / 2f + Vector3.right * -10,
            start + Vector3.up * length / 2f + Vector3.right * 10);

        Gizmos.DrawLine(
            start - Vector3.up * length / 2f + Vector3.right * -10,
            start - Vector3.up * length / 2f + Vector3.right * 10);



        Camera cam = GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Camera>();
        float cameraWidth = cam.orthographicSize*2f;

        Vector3 origin = transform.position + Vector3.left * 50;

        float up_leaveHeight = cam.transform.position.y + (cameraWidth + length)/2f;
        float up_respawnHeight = cam.transform.position.y - (length - cameraWidth)/2f;
        float down_leaveHeight = cam.transform.position.y - (cameraWidth + length)/2f;
        float down_respawnHeight = cam.transform.position.y + (length - cameraWidth)/2f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector2.up * up_leaveHeight + Vector2.right * 10, Vector2.one * length);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector2.up * up_respawnHeight + Vector2.right * 10, Vector2.one * length);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector2.up * down_leaveHeight - Vector2.right * 10, Vector2.one * length);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector2.up * down_respawnHeight - Vector2.right * 10, Vector2.one * length);
    }
    */
}
