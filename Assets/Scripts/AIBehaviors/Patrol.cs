using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : AIBehavior
{
    public override BehaviorType behaviorType {get {return BehaviorType.CONTINUOUS;}}
    [SerializeField] Vector2[] patrolPoints;
    [SerializeField] float waitTimer = 1f;
    bool patrolActive = true;
    int goal = 0;
    

    Vector2 currentPos = Vector2.zero;
    Vector2 heading = Vector2.zero;

    

    EntityController ec;

    // Start is called before the first frame update
    void Start()
    {
        ec = GetComponent<EntityController>();
        goal = 0;
    }

    public override void Act()
    {
        currentPos = transform.position;
        heading.x = currentPos.x - patrolPoints[goal].x;
        heading.y = currentPos.y - patrolPoints[goal].y;

        float distance = heading.x * heading.x;
        distance = Mathf.Sqrt(distance);
        if (distance < 0.1f) {
            goal = (goal + 1) % (patrolPoints.Length);
            patrolActive = false;
            StartCoroutine(Wait());
        }

        if (patrolActive) {
            ec.motionInput.x = (heading.x > 0) ? -1 : 1;
        } else {
            ec.motionInput.x = 0;
        }
        
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(waitTimer);
        patrolActive = true;
    }

    #if UNITY_EDITOR
    Vector2 gizmosCubeSize = new Vector2(0.5f, 0.5f);
    void OnDrawGizmosSelected()
    {
        if (patrolPoints.Length == 0) return;
        Gizmos.color = Color.cyan;
        Vector2 prev = patrolPoints[0];
        Gizmos.DrawCube(prev, gizmosCubeSize);
        Gizmos.color = Color.yellow;
        if (patrolPoints.Length < 1) return;
        for (int i = 1; i < patrolPoints.Length; i++) {
            Vector2 next = patrolPoints[i];
            Gizmos.DrawCube(next, gizmosCubeSize);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
    #endif
}
