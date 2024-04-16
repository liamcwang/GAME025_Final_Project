using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hurtbox : MonoBehaviour
{
    public float maxHealth = 10f;
    public float health = 10f;
    [SerializeField] bool useNativeDeath = true;
    private Vector2 bounceForce = Vector2.zero;

    public event Action onTakeDamage;

    [SerializeField] float flashTimer = 0.125f;
    private static Material flashMGlobal;
    private static Material flashMatGlobal {
        get {
            if (flashMGlobal == null) {
                flashMGlobal = Resources.Load<Material>("Materials/Flash");
            }
            return flashMGlobal;
        } 
        set {
            flashMGlobal = value;
        }
    }
    private Material flashMat;
    bool flashActive;

    Rigidbody2D rb;
    List<SpriteRenderer> allSprites;
    Player player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        allSprites = new List<SpriteRenderer>();
        // SpriteRenderer spr = GetComponent<SpriteRenderer>();
        // if (spr != null) {
        //     allSprites.Add(spr);
        // }
        
        SpriteRenderer[] childSprites = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < childSprites.Length; i++) {
            allSprites.Add(childSprites[i]);
        }
        flashActive = false;

        player = GetComponent<Player>();
        flashMat = new Material(flashMatGlobal);
    }

    public void takeDamage(float n, bool useFlash = true) {
        health -= n;
        if (allSprites.Count > 0 && !flashActive && useFlash) {
            flashActive = true;
            StartCoroutine(DamageFlash(flashTimer));
        }

        onTakeDamage?.Invoke();
        
        if (useNativeDeath && health < 0.01) {
            Die();
        }
    }

    public void takeDamageBounce(float n, Vector2 origin, Vector2 bounce, float bounceTimer) {
        takeDamage(n, false);
        if (player != null) player.loseControl(bounceTimer);
        
        float diff = transform.position.x - origin.x;

        if (diff > 0) {
            bounceForce.x = bounce.x;
        } else {
            bounceForce.x = -bounce.x;
        }
        bounceForce.y = bounce.y;
        //Debug.Log(bounceForce);
        rb.velocity = bounceForce;
    }

    void Die() {
        Destroy(gameObject);
    }

    IEnumerator DamageFlash(float timer) {
        int allSpriteLength = allSprites.Count;
        SpriteRenderer[] spriteArray = new SpriteRenderer[allSpriteLength];
        Material[] originalArray = new Material[allSpriteLength];
        for(int i = 0; i < allSpriteLength; i++) {
            SpriteRenderer sprite = allSprites[i];
            //Debug.Log(sprite);
            spriteArray[i] = sprite;
            originalArray[i] = sprite.material;
            sprite.material = flashMat;
        }
        yield return new WaitForSeconds(timer);
        for (int i = 0; i < allSpriteLength; i++) {
            SpriteRenderer sprite = spriteArray[i];
            sprite.material = originalArray[i];
        }
        flashActive = false;
    }
}
