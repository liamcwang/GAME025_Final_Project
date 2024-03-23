using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashSpeed = 40f;
    public float dashTimer = 0.25f;

    Hurtbox hurtbox;
    Animator anim;
    Rigidbody2D rb;
    EntityController ec;
    Vector2 motionVector = Vector2.zero;
    Collider2D[] colliders;
    

    void Start() {
        hurtbox = GetComponent<Hurtbox>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ec = GetComponent<EntityController>();
        colliders = GetComponents<Collider2D>();
    }

    public void Act(float horizontalInput)
    {
        ec.state = ActionState.DASH;
        anim.Play("dash");
        motionVector.x = horizontalInput < 0 ? -dashSpeed : dashSpeed;

        
        foreach (Collider2D coll in colliders) {
            coll.enabled = false;
        }
        rb.velocity = motionVector;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        
        StartCoroutine(DashDuration());
    }

    IEnumerator DashDuration() {
        yield return new WaitForSeconds(dashTimer);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        ec.stateOverride = false;
        foreach (Collider2D coll in colliders) {
            coll.enabled = true;
        }
    }
}
