using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private const float Z_POS = -10f;

    public Vector2 offset = Vector2.zero;
    public float trackingFactor = 1f;
    public Vector3 newPosition = Vector3.zero;
    public bool lerpTrackingEnabled = false;

    private GameObject player;

    private void Awake()
    {
        GameManager.Instance.PlayerCamera = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player.gameObject;
        newPosition.z = Z_POS;
    }

    // Update is called once per frame
    void Update()
    {
        // CHANGING THE RIGIDBODIES TO DYNAMIC AND FREEZING THE X Y AXES FIXES IT??
        float yCurrentPos = transform.position.y - offset.y;
        float yPlayerPos = player.transform.position.y;

        /*if (player.GetComponent<Movement>().jumpCount > 0 && yPlayerPos > yCurrentPos) {
            newPosition.x = player.transform.position.x;
            newPosition.y = transform.position.y;
        } else {
            newPosition.x = player.transform.position.x;
            newPosition.y = player.transform.position.y + offset.y;
        }*/

        newPosition.x = player.transform.position.x;
        newPosition.y = player.transform.position.y + offset.y;

        if (lerpTrackingEnabled) {
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * trackingFactor);
        } else {
            transform.position = newPosition;
        }
        
        //transform.position = newPosition;   
    }
}
