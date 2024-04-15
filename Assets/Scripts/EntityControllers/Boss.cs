using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Boss : MonoBehaviour
{
    public enum Methods {LERP_MOVE, WAIT, SHOOT_PLAYER};

    public ActionPattern[] actionPatterns;
    private float waitTimer;
    private int actionPointer;
    private Vector2 destVector = Vector2.zero;
    private Vector3 heading = Vector3.zero;
    private Rigidbody2D rb;
    private ProjectileSpawner projectileSpawner;
    private MethodInfo currentMethod;
    private int currentMethodPointer;
    private Player playerRef;
    private object[][] currentMethodArgs = new object[Enum.GetNames(typeof(Methods)).Length][];

    private void Awake()
    {

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
        playerRef = GameManager.Instance.Player;
        interpretCall(actionPatterns[actionPointer].methodCall);
    }

    private void Update()
    {
        bool result = (bool) currentMethod.Invoke(this, currentMethodArgs[currentMethodPointer]);
        if (result) {
            actionPointer = (actionPointer + 1) % actionPatterns.Length;
            interpretCall(actionPatterns[actionPointer].methodCall);
        }
    }

    private void interpretCall(MethodCall methodCall) {
        // MethodInfo methodInfo;
        object[] args = new object[methodCall.arguments.Length];
        switch(methodCall.method) {
            case Methods.LERP_MOVE:
                for (int i = 0; i < methodCall.arguments.Length; i++) {
                    args[i] = Convert.ToSingle(methodCall.arguments[i]);
                }

                destVector.x = (float) args[0];
                destVector.y = (float) args[1];
                currentMethod = this.GetType().GetMethod("MoveToPoint");
                currentMethodPointer = (int) Methods.LERP_MOVE;
                currentMethodArgs[currentMethodPointer] = new object[]{args[2]}; // run from it, dread it, the GC comes for us all
                break;
            case Methods.WAIT:
                waitTimer = Convert.ToSingle(methodCall.arguments[0]);
                currentMethod = this.GetType().GetMethod("Wait");
                currentMethodPointer = (int) Methods.WAIT;
                break;
            case Methods.SHOOT_PLAYER:


                int numShots = (int) int.Parse(methodCall.arguments[0]);
                float shootDelay = (float) Convert.ToSingle(methodCall.arguments[1]);
                StartCoroutine(ShootAtPlayer(numShots, shootDelay));
                // not my intended way of doing this, but I can't remember what my plan was
                waitTimer = numShots * shootDelay; 
                currentMethod = this.GetType().GetMethod("Wait");
                currentMethodPointer = (int) Methods.WAIT;
                break;
            default:
                break;
        }
        return;
    }

    public bool MoveToPoint(float lerpFactor) {
        rb.MovePosition(Vector2.Lerp(transform.position, destVector, Time.deltaTime * lerpFactor));

        heading.x = (destVector.x - transform.position.x);
        heading.y = (destVector.y - transform.position.y);

        float distanceSquared = heading.x * heading.x + heading.y * heading.y;
        float distance = Mathf.Sqrt(distanceSquared);
        
        return distance <= 0.1f;
    }

    public bool Wait() {
        waitTimer -= Time.deltaTime;
        return waitTimer <= 0.01f;
    }

    IEnumerator ShootAtPlayer(int numShots, float shootDelay) {
        Vector3 aimVector = Vector3.zero;
        Quaternion aimRotation = Quaternion.identity;
        Vector2 heading = Vector2.zero;


        for (int i = 0; i< numShots; i++) {
            heading.x = playerRef.transform.position.x - transform.position.x;
            heading.y = playerRef.transform.position.y - transform.position.y;

            aimVector.z = (float) (Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg);
            Debug.Log($"{aimVector.z}");

            aimRotation.eulerAngles = aimVector;
            Debug.Log($"{aimRotation.eulerAngles}");
            projectileSpawner.spawnProjectile(transform.position, aimRotation);

            yield return new WaitForSeconds(shootDelay);
        }
    }

}

[System.Serializable]
public struct ActionPattern {
    public string name;
    public MethodCall methodCall;
}

[System.Serializable]
public struct MethodCall {
    public Boss.Methods method;
    public string[] arguments;

}