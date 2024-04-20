using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public float damage;

    [SerializeField] Vector2 boxSize = Vector2.zero;
    [SerializeField] Vector2 offset = Vector2.zero;
    Vector3 boxSize3D = Vector3.zero;
    Vector2 direction = Vector2.right;
    Vector2 dest = Vector2.zero;
    Vector2 visualDest = Vector2.zero;
    SpriteRenderer sprite;
    Animator anim;
    EntityController entity;

    [SerializeField]bool hitBoxActive = false;
    List<int> hitRecord = new List<int>();

    [SerializeField] LayerMask ignoreMask = Physics2D.IgnoreRaycastLayer;
    [SerializeField] bool bounceAttack = false;
    [SerializeField] Vector2 bounceForce = new Vector2(10f,10f);
    [SerializeField] float bounceTimer = 0.5f;

    private void Start()
    {
        hitBoxActive = false;
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        entity = GetComponent<EntityController>();
    }

    public void Act()
    {
        entity.stateOverride = true;
        hitRecord.Clear();
        anim.Play("attack");
    }

    private void Update()
    {
        if (hitBoxActive) HitBoxCast();
    }

    public void HitBoxCast() {
        float xDir = sprite.flipX ? -1 : 1;
        direction.x = xDir;
        dest.x = transform.position.x + (offset.x * direction.x);
        dest.y = transform.position.y + offset.y;
        // Cast a box at an arbitrary point in space
        // seems to be inconsistent when dealing with tilemaps, needs to be studied
        // Confounding bug where it'll contact with the tilemap and stop finding further collisions
        // solution is to ignore the tilemaps
        RaycastHit2D[] hits = Physics2D.BoxCastAll(dest, boxSize, 0f, direction, 0.1f, ~ignoreMask);
        foreach (RaycastHit2D hit in hits) {
            // Hashmaps seems like a potential solution for quickly storing what got hit
            if (hit.collider != null) {
                Hurtbox hurtbox = hit.collider.gameObject.GetComponent<Hurtbox>();
                if (hurtbox == null) return;
                
                int objectID = hit.collider.gameObject.GetInstanceID();
                
                if (!hitRecord.Contains(objectID)) {
                    hitRecord.Add(objectID);
                    if (bounceAttack) {
                        hurtbox.takeDamageBounce(damage, transform.position, bounceForce, bounceTimer);
                    } else {
                        hurtbox.takeDamage(damage);
                    }
                    
                    Debug.Log(hit.collider.name);
                }
            }
        }
    }

    public bool SimulateCast() {
        float xDir = sprite.flipX ? -1 : 1;
        direction.x = xDir;
        dest.x = transform.position.x + (offset.x * direction.x);
        dest.y = transform.position.y + offset.y;
        // Cast a box at an arbitrary point in space
        // seems to be inconsistent when dealing with tilemaps, needs to be studied
        // Confounding bug where it'll contact with the tilemap and stop finding further collisions
        // solution is to ignore the tilemaps
        RaycastHit2D[] hits = Physics2D.BoxCastAll(dest, boxSize, 0f, direction, 0.1f, ~ignoreMask);
        foreach (RaycastHit2D hit in hits) {
            // Hashmaps seems like a potential solution for quickly storing what got hit
            if (hit.collider.CompareTag("Player")) return true;
            
        }

        return false;
    }

    public void activate() {
        hitBoxActive = true;
    }

    /// <summary>
    /// State override occurs during the end frames of the animation,
    /// Place it at the end of the animation clip where the animation should end
    /// </summary>
    public void deactivate() {
        hitBoxActive = false;
        if (entity != null) {
            entity.stateOverride = false;
        }
        
    }

    /// <summary>
    /// Hopefully these Gizmos are representing the hitbox as it actually works
    /// </summary>
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) {
            boxSize3D.x = boxSize.x;
            boxSize3D.y = boxSize.y;
        }
        Gizmos.color = hitBoxActive ? Color.green : Color.clear;

        if (!Application.isPlaying) {
            visualDest.x = transform.position.x + (offset.x * direction.x);
            visualDest.y = transform.position.y + offset.y;
        }
        Gizmos.DrawWireCube(visualDest, boxSize3D);
    }
}
