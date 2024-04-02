using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalProjectileAttack : AIBehavior
{
    float projectileTimer = 2f;
    public Vector2 spawnOffset = Vector2.zero;
    SpriteRenderer spriteRenderer;
    ProjectileSpawner projectileSpawner;

    private Vector2 spawnPoint = Vector2.zero;
    private Vector3 directionVector = Vector3.zero;
    private Quaternion spawnDirection = Quaternion.identity;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
        StartCoroutine(ShootProjectileInterval());

    }

    public override void Act() {

    }
    
    IEnumerator ShootProjectileInterval() {
        while(true) {
            directionVector.z = spriteRenderer.flipX ? 90 : -90;
            spawnDirection.eulerAngles = directionVector;
            spawnPoint.x = spriteRenderer.flipX ? transform.position.x - spawnOffset.x : transform.position.x + spawnOffset.x;
            spawnPoint.y = transform.position.y + spawnOffset.y;
            projectileSpawner.spawnProjectile(spawnPoint, spawnDirection);
            yield return new WaitForSeconds(projectileTimer);
        }
        

    }
}