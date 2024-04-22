using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] GameObject item;
    Hurtbox hurtbox;

    private void Start()
    {
        hurtbox = GetComponent<Hurtbox>();
        hurtbox.onDie += spawnHealth;
    }

    void spawnHealth() {
        Instantiate(item, transform.position, Quaternion.identity);
    }
    
}
