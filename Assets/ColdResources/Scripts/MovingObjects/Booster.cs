using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Booster : MonoBehaviour
{
    List<int> collidedWith = new ();

    public bool Boost(int id, float position, out float boostAmount)
    {
        boostAmount = 0f;
        if (collidedWith.Contains(id)) return false;

        var bounds = GetComponent<CapsuleCollider2D>().bounds;
        boostAmount = Mathf.InverseLerp(bounds.center.y - bounds.size.y / 2f, bounds.center.y + bounds.size.y / 2f, position);
        boostAmount = Mathf.Min(boostAmount, 1);

        if (boostAmount > 0f) {
            return true;
        } else {
            boostAmount = 0f;
            return false;
        }
    }

    private void OnDrawGizmosSelected ()
    {

        var collider = GetComponent<CapsuleCollider2D>();

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(collider.bounds.center, Vector2.one * collider.bounds.size);
    }
}
