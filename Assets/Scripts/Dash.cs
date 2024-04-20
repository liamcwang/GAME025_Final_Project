using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashSpeed = 40f;
    public float dashTimer = 0.25f;
    public int dashLimit = 1;
    private int dashCount = 0;

    Hurtbox hurtbox;
    Animator anim;
    Rigidbody2D rb;
    EntityController entity;
    Vector2 motionVector = Vector2.zero;
    Collider2D[] colliders;
    

    void Start() {
        hurtbox = GetComponent<Hurtbox>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        entity = GetComponent<EntityController>();
        colliders = GetComponents<Collider2D>();
    }
    // TODO: Fix sprite being able to dash backwards
    public void Act(float horizontalInput)
    {
        if (dashCount < dashLimit) {
            entity.stateOverride = true;

            entity.state = ActionState.DASH;
            anim.Play("dash");
            motionVector.x = horizontalInput < 0 ? -dashSpeed : dashSpeed;

            dashCount++;
            
            foreach (Collider2D coll in colliders) {
                coll.enabled = false;
            }
            rb.velocity = motionVector;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            
            StartCoroutine(DashDuration());
        }
        
        
    }

    IEnumerator DashDuration() {
        yield return new WaitForSeconds(dashTimer);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        entity.stateOverride = false;
        foreach (Collider2D coll in colliders) {
            coll.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            dashCount = 0;
        }
    }
}
