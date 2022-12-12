using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(ParallaxPaner))]
public class ParallaxInitializer : MonoBehaviour
{
    public Sprite[] sprites;

    private void Awake ()
    {
        Destroy(this);
    }

    [Button]
    void Setup ()
    {
        float totalLength = 0;
        int loopBreak = 1000;

        int layerOrder;

        while (transform.childCount > 0 && loopBreak-- > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        if (sprites.Length > 0) {

            float baseLength = sprites[0].bounds.size.y;
            float basePos = transform.position.y;

            if (TryGetComponent(out SpriteRenderer spriteRenderer)) {
                spriteRenderer.sprite = sprites[0];
                layerOrder = spriteRenderer.sortingOrder;
            }
            else {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprites[0];
                layerOrder = spriteRenderer.sortingOrder;
            }

            float lastLength = baseLength;
            float lastPos = transform.position.y;

            totalLength = baseLength;
            for (int i = 1; i < sprites.Length; i++) {
                Sprite sprite = sprites[i];
                totalLength += sprite.bounds.size.y;
                spriteRenderer = new GameObject("texture - " + i, typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = layerOrder;
                spriteRenderer.sprite = sprite;

                float pos = lastPos - lastLength/2f - sprite.bounds.size.y/2f;
                spriteRenderer.transform.parent = transform;
                spriteRenderer.transform.position = new Vector3(
                    transform.position.x, 
                    pos, 
                    transform.position.z);

                lastPos = pos;
                lastLength = sprite.bounds.size.y;
            }

            lastPos = basePos + totalLength;
            lastLength = 0;
            for (int i = 0; i < sprites.Length; i++) {
                Sprite sprite = sprites[i];
                spriteRenderer = new GameObject("texture Before - " + i, typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = layerOrder;
                spriteRenderer.sprite = sprite;

                float pos = lastPos - lastLength/2f - (i>0 ? (sprite.bounds.size.y/2f) : 0);
                spriteRenderer.transform.parent = transform;
                spriteRenderer.transform.position = new Vector3(
                    transform.position.x,
                    pos,
                    transform.position.z);

                lastPos = pos;
                lastLength = sprite.bounds.size.y;
            }

            lastPos = basePos - totalLength;
            lastLength = 0;
            for (int i = 0; i < sprites.Length; i++) {
                Sprite sprite = sprites[i];
                spriteRenderer = new GameObject("texture After - " + i, typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = layerOrder;
                spriteRenderer.sprite = sprite;

                float pos = lastPos - lastLength/2f - (i>0 ? (sprite.bounds.size.y/2f) : 0);
                spriteRenderer.transform.parent = transform;
                spriteRenderer.transform.position = new Vector3(
                    transform.position.x,
                    pos,
                    transform.position.z);

                lastPos = pos;
                lastLength = sprite.bounds.size.y;
            }




        } else if (TryGetComponent(out SpriteRenderer spriteRenderer)) {
            spriteRenderer.sprite = null;
        }
        var paner = GetComponent<ParallaxPaner>();
        paner.length = totalLength;
    }
}
