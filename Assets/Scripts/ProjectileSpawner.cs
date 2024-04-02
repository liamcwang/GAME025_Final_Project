using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectile;

    public void spawnProjectile(Vector3 position, Quaternion direction) {
        Instantiate(projectile, position, direction);
    }

}