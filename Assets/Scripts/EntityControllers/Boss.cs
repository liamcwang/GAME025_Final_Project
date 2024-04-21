using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Boss : EntityController
{
    public enum Methods {TELEPORT, WAIT, SHOOT_PLAYER, MOVE_AND_ATTACK};

    public ActionPattern[] actionPatterns;
    private float waitTimer;
    private int actionPointer;
    private Vector2 destVector = Vector2.zero;
    private Vector3 heading = Vector3.zero;
    
    private MethodInfo currentMethod;
    private int currentMethodPointer;
    private Player playerRef;
    private object[][] currentMethodArgs = new object[Enum.GetNames(typeof(Methods)).Length][]; // create an array to reference args
    private Vector3 aimVector = Vector3.zero;
    [SerializeField] float turnAroundTimer = 1f;

    private Rigidbody2D rb;
    private ProjectileSpawner projectileSpawner;
    Movement movement;
    ReactiveAttack reactAttack;
    Animator anim;
    Hurtbox hurtbox;

    private void Awake()
    {

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
        movement = GetComponent<Movement>();
        reactAttack = GetComponent<ReactiveAttack>();
        anim = GetComponent<Animator>();
        hurtbox = GetComponent<Hurtbox>();

        playerRef = GameManager.Player;
        interpretCall(actionPatterns[actionPointer].methodCall);
    }

    private void FixedUpdate()
    {
        if (hurtbox.health <= 0.1) Die();
        bool result = (bool) currentMethod.Invoke(this, currentMethodArgs[currentMethodPointer]);
        if (result) {
            actionPointer = (actionPointer + 1) % actionPatterns.Length;
            interpretCall(actionPatterns[actionPointer].methodCall);
        }
    }

    void Die() {
        GameManager.Victory();
        hurtbox.Die();
    }

    /// <summary>
    /// Method to call a specific set of arbitrary arguments defined in Boss
    /// </summary>
    /// <param name="methodCall"></param>
    private void interpretCall(MethodCall methodCall) {
        // MethodInfo methodInfo;
        object[] args = new object[methodCall.arguments.Length];
        switch(methodCall.method) {
            // Teleport(float x, float y)
            case Methods.TELEPORT:
                for (int i = 0; i < methodCall.arguments.Length; i++) {
                    args[i] = Convert.ToSingle(methodCall.arguments[i]);
                }

                destVector.x = (float) args[0];
                destVector.y = (float) args[1];
                currentMethod = this.GetType().GetMethod("Teleport");
                currentMethodPointer = (int) Methods.TELEPORT;
                break;
            
            // Wait(float timer)
            case Methods.WAIT:
                waitTimer = Convert.ToSingle(methodCall.arguments[0]);
                currentMethod = this.GetType().GetMethod("Wait");
                currentMethodPointer = (int) Methods.WAIT;
                break;

            // ShootPlayer(int numShots, float shootDelay)
            case Methods.SHOOT_PLAYER:
                int numShots = (int) int.Parse(methodCall.arguments[0]);
                float shootDelay = (float) Convert.ToSingle(methodCall.arguments[1]);
                StartCoroutine(ShootAtPlayer(numShots, shootDelay));
                // not my intended way of doing this, but I can't remember what my plan was
                waitTimer = numShots * shootDelay; 
                currentMethod = this.GetType().GetMethod("Wait");
                currentMethodPointer = (int) Methods.WAIT;
                break;
            
            // MoveAndAttack(float timer)
            case Methods.MOVE_AND_ATTACK:
                waitTimer = Convert.ToSingle(methodCall.arguments[0]);
                currentMethod = this.GetType().GetMethod("MoveAndAttack");
                currentMethodPointer = (int) Methods.MOVE_AND_ATTACK;
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

    public bool Teleport() {
        rb.MovePosition(destVector);
        return true;
    }

    public bool Wait() {
        waitTimer -= Time.fixedDeltaTime;
        return waitTimer <= 0.01f;
    }

    public bool MoveAndAttack() {
        waitTimer -= Time.fixedDeltaTime;
        bool actionFinished = waitTimer <= 0.01f;

        if (!actionFinished) {
            if (stateOverride) return false;
            ActionState newState = ActionState.NONE;
            heading.x = GameManager.Player.transform.position.x - transform.position.x;
            float newMotion = heading.x > 0.1 ? 1f : -1f;
            if (newMotion != motionInput.x) {
                StartCoroutine(overrideTimer(turnAroundTimer));
            } 
            motionInput.x = newMotion;
            
            reactAttack.Act();

            if (!stateOverride)  {
                newState = movement.Move(motionInput);
                state = newState;
                anim.SetInteger("ActionState", (int) state); 
            }
        }
        

        return actionFinished;
    }

    IEnumerator overrideTimer(float timer) {
        Debug.Log($"override!");
        stateOverride = true;
        anim.SetInteger("ActionState", (int) ActionState.IDLE);
        yield return new WaitForSeconds(timer);
        stateOverride = false;
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