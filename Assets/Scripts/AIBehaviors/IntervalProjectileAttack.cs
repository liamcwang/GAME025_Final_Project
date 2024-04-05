using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This behavior shoots a projectile at fixed intervals
/// </summary>
public class IntervalProjectileAttack : AIBehavior
{
    public override BehaviorType behaviorType {get {return BehaviorType.ON_START;}}
    float projectileTimer = 2f;
    public float shootAngle = 90f; // angle in degrees
    public Vector2 spawnOffset = Vector2.zero;

    SpriteRenderer spriteRenderer;
    ProjectileSpawner projectileSpawner;
    private Vector2 spawnPoint = Vector2.zero;
    private Vector3 fireAngleVector = Vector3.zero; 
    private Quaternion spawnDirection = Quaternion.identity;
    private float xDir = 1;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
    }

    public override void Act() {
        StartCoroutine(ShootProjectileInterval());
    }
    
    private void Update()
    {
        
    }

    // TODO: Make enemy stop shooting when state is overridden, then resume.
    IEnumerator ShootProjectileInterval() {
        while(true) {
            yield return new WaitForSeconds(projectileTimer);
            // bc of Unity, the angles must be set in a 3D vector
            // then transformed back into Quaternion, fun
            fireAngleVector.z = spriteRenderer.flipX ? shootAngle : -shootAngle;
            spawnDirection.eulerAngles = fireAngleVector;
            xDir = spriteRenderer.flipX ? -1: 1;
            spawnPoint.x = transform.position.x + (spawnOffset.x * xDir);
            spawnPoint.y = transform.position.y + spawnOffset.y;
            projectileSpawner.spawnProjectile(spawnPoint, spawnDirection);
        }
    }

    #if UNITY_EDITOR
    Vector2 gizmosCubeSize = new Vector2(0.4f, 0.4f);
    Vector3 firingDirection = Vector3.zero;

    /// <summary>
    /// To draw where the projectiles start from
    /// and the direction the projectiles will go
    /// </summary>
    void OnDrawGizmosSelected() {
        if (!Application.isPlaying) {
            spawnPoint.x = transform.position.x + (spawnOffset.x * xDir);
            spawnPoint.y = transform.position.y + spawnOffset.y;

            fireAngleVector.z = -shootAngle; // this assumes you're facing right
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(spawnPoint, gizmosCubeSize);

        double radian = (fireAngleVector.z) * Math.PI/180;
        firingDirection.x = transform.position.x - (float) Math.Sin(radian);
        firingDirection.y = transform.position.y + (float) Math.Cos(radian);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(spawnPoint, firingDirection);
    }
    #endif
}