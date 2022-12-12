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
        //startOffset = 0;
    }

    private void OnDrawGizmosSelected ()
    {
        Vector3 start = transform.position;// + Vector3.up * startOffset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            start + Vector3.up * length / 2f + Vector3.right * -10,
            start + Vector3.up * length / 2f + Vector3.right * 10);

        Gizmos.DrawLine(
            start - Vector3.up * length / 2f + Vector3.right * -10,
            start - Vector3.up * length / 2f + Vector3.right * 10);
    }

    private void Start ()
    {
        startPos = transform.position.y;
        cam = Camera.main.gameObject;
    }

    private void Update ()
    {
        float t = cam.transform.position.y * (1-parallaxAmount);
        float dist = cam.transform.position.y * parallaxAmount;

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
