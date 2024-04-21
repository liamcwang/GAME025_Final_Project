using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] Vector2 spawnOffset;
    [SerializeField] Vector2 playerEntranceOffset = Vector2.zero;
    [SerializeField] Camera BossCam;
    Vector2 spawnPoint = Vector2.zero;
    Vector2 playerEntrance = Vector2.zero;
    Vector3 cubeDim = new Vector3(1,1,1);
    bool isActive = true;

    private void Start()
    {
        spawnPoint.x = transform.position.x + spawnOffset.x;
        spawnPoint.y = transform.position.y + spawnOffset.y;
        BossCam.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isActive) {
            isActive = false; // otherwise we spawn more bosses than intended
            BossCam.enabled = true;
            other.gameObject.transform.position = playerEntrance;
            GameManager.PlayerCamera.GetComponent<Camera>().enabled = false;
            Instantiate(Boss, spawnPoint, Quaternion.identity);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        spawnPoint.x = transform.position.x + spawnOffset.x;
        spawnPoint.y = transform.position.y + spawnOffset.y;
        Gizmos.DrawCube(spawnPoint, cubeDim);

        Gizmos.color = Color.green;
        playerEntrance.x = transform.position.x + playerEntranceOffset.x;
        playerEntrance.y = transform.position.y + playerEntranceOffset.y;
        Gizmos.DrawCube(playerEntrance, cubeDim);
    }
}
