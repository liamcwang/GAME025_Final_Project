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
    private Vector3 aimVector = Vector3.zero;

    private void Awake()
    {

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
        playerRef = GameManager.Player;
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
        Quaternion aimRotation = Quaternion.identity;
        Vector3 heading = Vector2.zero;


        for (int i = 0; i< numShots; i++) {
            heading.x = playerRef.transform.position.x - transform.position.x;
            heading.y = playerRef.transform.position.y - transform.position.y;
            float hypo = heading.x * heading.x + heading.y * heading.y;
            hypo = Mathf.Sqrt(hypo);
            heading.z = Mathf.Acos(heading.y/hypo);
            aimVector.z = (float) (heading.z * 180/Mathf.PI);
            aimVector.z = heading.x > 0 ? -aimVector.z : aimVector.z; // correct the angle based on the direction
            Debug.Log($"{aimVector.z}");

            aimRotation.eulerAngles = aimVector;
            Debug.Log($"{aimRotation.eulerAngles}");
            projectileSpawner.spawnProjectile(transform.position, aimRotation);

            yield return new WaitForSeconds(shootDelay);
        }
    }

    #if UNITY_EDITOR
    Vector2 gizmosCubeSize = new Vector2(0.4f, 0.4f);
    Vector3 firingDirection = Vector3.zero;

    /// <summary>
    /// To draw where the projectiles start from
    /// and the direction the projectiles will go
    /// </summary>
    void OnDrawGizmos() {

        Gizmos.color = Color.magenta;

        double radian = (aimVector.z) * Math.PI/180;
        firingDirection.x = transform.position.x - (float) Math.Sin(radian);
        firingDirection.y = transform.position.y + (float) Math.Cos(radian);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, firingDirection);
    }
    #endif
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