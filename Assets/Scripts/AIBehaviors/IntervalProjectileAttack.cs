using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntervalProjectileAttack : AIBehavior
{
    float projectileTimer = 2f;
    public float shootAngle = 90f; // angle in degrees
    public Vector2 spawnOffset = Vector2.zero;
    SpriteRenderer spriteRenderer;
    ProjectileSpawner projectileSpawner;
    private Vector2 spawnPoint = Vector2.zero;
    private Vector3 directionVector = Vector3.zero; 
    private Quaternion spawnDirection = Quaternion.identity;
    private float xDir = 1;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
        StartCoroutine(ShootProjectileInterval());

    }

    public override void Act() {

    }
    
    IEnumerator ShootProjectileInterval() {
        while(true) {
            directionVector.z = spriteRenderer.flipX ? shootAngle : -shootAngle;
            spawnDirection.eulerAngles = directionVector;
            xDir = spriteRenderer.flipX ? -1: 1;
            spawnPoint.x = transform.position.x + (spawnOffset.x * xDir);
            spawnPoint.y = transform.position.y + spawnOffset.y;
            projectileSpawner.spawnProjectile(spawnPoint, spawnDirection);
            yield return new WaitForSeconds(projectileTimer);
        }
        

    }

    #if UNITY_EDITOR
    Vector2 gizmosCubeSize = new Vector2(0.4f, 0.4f);
    Vector3 firingDirection = Vector3.zero;
    void OnDrawGizmosSelected() {
        if (!Application.isPlaying) {
            spawnPoint.x = transform.position.x + (spawnOffset.x * xDir);
            spawnPoint.y = transform.position.y + spawnOffset.y;

            directionVector.z = shootAngle;
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(spawnPoint, gizmosCubeSize);

        double radian = (directionVector.z) * Math.PI/180;
        firingDirection.x = transform.position.x + (float) Math.Sin(radian);
        firingDirection.y = transform.position.y + (float) Math.Cos(radian);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(spawnPoint, firingDirection);
    }
    #endif
}