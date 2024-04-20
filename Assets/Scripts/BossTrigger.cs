using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] Vector2 spawnOffset;
    Vector2 spawnPoint = Vector2.zero;
    Vector3 cubeDim = new Vector3(1,1,1);

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(Boss, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        spawnPoint.x = transform.position.x + spawnOffset.x;
        spawnPoint.y = transform.position.y + spawnOffset.y;
        Gizmos.DrawCube(spawnPoint, cubeDim);
    }
}
