using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectile;
    [SerializeField] LayerMask ignoreMask = Physics2D.IgnoreRaycastLayer;

    public void spawnProjectile(Vector3 position, Quaternion direction) {
        GameObject gObj = Instantiate(projectile, position, direction);
        Projectile pComponent = gObj.GetComponent<Projectile>();
    }
}