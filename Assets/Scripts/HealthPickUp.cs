using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public float healAmount = 2f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Player p = other.gameObject.GetComponent<Player>();
            p.hurtbox.heal(healAmount);
            Destroy(gameObject);
        }
    }
}
