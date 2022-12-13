using NaughtyAttributes;
using UnityEngine;

[SelectionBase]
public class ParallaxPaner : MonoBehaviour
{
    private float startPos;
    GameObject cam;

    public float parallaxAmount;
    public float length;
    //public float startOffset;

    public bool isArriving = true;
    public bool isLooping = true;
    public bool isLeaving = false;

    [Button]
    void AutoLength ()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

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
    }

    float _offset;
    private void Start ()
    {
        startPos = transform.position.y;
        cam = Camera.main.gameObject;
        _offset = cam.transform.position.y;
    }

    private void Update ()
    {
        float t = (cam.transform.position.y - _offset) * (1-parallaxAmount);
        float dist = (cam.transform.position.y - _offset) * parallaxAmount;

        transform.position = new Vector3(transform.position.x, startPos + dist, transform.position.z);

        if (isArriving && (t < startPos + length && t > startPos - length))
            isArriving = false;

        if (isLooping && !(isArriving || isLeaving)) {
            if (t > startPos + length) startPos += length;
            else if (t < startPos - length) startPos -= length;
        }

        if (isLeaving && t > startPos + length)
            Destroy(gameObject);
    }
}
