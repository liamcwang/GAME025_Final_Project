using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] Vector2 boxSize;
    [SerializeField] Vector2 offset;
    [SerializeField] LayerMask hitMask; // must be set to ground layer in inspectore
    [HideInInspector] public bool touchingGround;

    private Vector2 dest;

    private void FixedUpdate()
    {
        groundCheckCast();
    }

    // RESEARCH: How not to raycast every frame
    private void groundCheckCast() {
        dest.x = transform.position.x + offset.x;
        dest.y = transform.position.y + offset.y;

        RaycastHit2D hit = Physics2D.BoxCast(dest, boxSize, 0f, Vector2.zero, 0f, hitMask);
        // is a null check even safe?
        touchingGround = hit.collider != null;
    }

    Vector3 boxSize3D = Vector3.zero;
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) {
            boxSize3D.x = boxSize.x;
            boxSize3D.y = boxSize.y;
        }
        Gizmos.color = Color.yellow;

        if (!Application.isPlaying) {
            dest.x = transform.position.x + offset.x;;
            dest.y = transform.position.y + offset.y;;
        }
        Gizmos.DrawWireCube(dest, boxSize3D);
    }
}