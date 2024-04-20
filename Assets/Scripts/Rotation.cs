using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Vector3 directionVector = Vector3.zero;
    private Quaternion quat = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        quat.eulerAngles = directionVector;
        transform.rotation = quat;
        
        
    }
}
