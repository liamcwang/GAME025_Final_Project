using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    GameObject item;

    private void OnDisable()
    {
        Instantiate(item, transform.position, Quaternion.identity);
    }
}
