using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Projectiles in game are built from here.
/// Super modular foundation to have a bunch of different functions 
/// with reused code
/// </summary>
public class Projectile : MonoBehaviour
{
    public float speed = 20;
    public float damage = 1;
    public float decayTimer = 5f;
    public LayerMask ignoreMask;
    public LayerMask ignoreDamageMask;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
        if (decayTimer > 0) { // this means we can disable the decay from happening
            StartCoroutine(Decay(decayTimer));
        }
        
    }


    
    /// <summary>
    /// Coroutine to destroy the projectile after some time
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    private IEnumerator Decay(float f) {
        yield return new WaitForSeconds(f);
        Destroy(gameObject);
    }

    /// <summary>
    /// Projectiles are responsible for calling damage-related functions 
    /// on other objects.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        // checking layer collision, if it's in the ignoreMask, ignore it
        if ((ignoreMask.value & (1 << other.gameObject.layer)) == 0) {
            Hurtbox hurtBox = other.gameObject.GetComponent<Hurtbox>();
            if (hurtBox != null && (ignoreDamageMask.value & (1 << other.gameObject.layer)) == 0) {
                hurtBox.takeDamage(damage);
            }
            Destroy(gameObject);

        }
    }

}
