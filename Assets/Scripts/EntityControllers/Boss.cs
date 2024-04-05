using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Boss : MonoBehaviour
{
    public enum Methods {LERP_MOVE, WAIT};

    public ActionPattern[] actionPatterns;
    private float waitTimer;
    private int actionPointer;
    private Vector2 destVector = Vector2.zero;
    private Vector3 heading = Vector3.zero;
    private Rigidbody2D rb;

    private MethodInfo currentMethod;
    private object[] currentMethodArgs;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        interpretCall(actionPatterns[actionPointer].methodCall);
    }

    private void Update()
    {
        bool result = (bool) currentMethod.Invoke(this, currentMethodArgs);
        if (result) {
            actionPointer = (actionPointer + 1) % actionPatterns.Length;
            interpretCall(actionPatterns[actionPointer].methodCall);
        }
    }

    private void interpretCall(MethodCall methodCall) {
        MethodInfo mi;

        switch(methodCall.method) {
            case Methods.LERP_MOVE:
                object[] args = new object[methodCall.arguments.Length];
                for (int i = 0; i < methodCall.arguments.Length; i++) {
                    args[i] = Convert.ToSingle(methodCall.arguments[i]);
                }

                destVector.x = (float) args[0];
                destVector.y = (float) args[1];
                currentMethod = this.GetType().GetMethod("MoveToPoint");
                currentMethodArgs = new object[]{(float) args[2]};
                break;
            case Methods.WAIT:
                waitTimer = Convert.ToSingle(methodCall.arguments[0]);
                currentMethod = this.GetType().GetMethod("Wait");
                currentMethodArgs = new object[0];
                break;
            default:
                break;
        }
        return;
    }

    public bool MoveToPoint(float lerpFactor) {
        transform.position = Vector2.Lerp(transform.position, destVector, Time.deltaTime * lerpFactor);

        heading.x = (destVector.x - transform.position.x);
        heading.y = (destVector.y - transform.position.y);

        float distanceSquared = heading.x * heading.x + heading.y * heading.y;
        float distance = Mathf.Sqrt(distanceSquared);
        
        return distance <= 0.1;
    }

    public bool Wait() {
        waitTimer -= Time.deltaTime;
        Debug.Log($"{waitTimer}");
        return waitTimer <= 0.01;
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