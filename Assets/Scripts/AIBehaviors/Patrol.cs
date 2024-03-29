using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : AIBehavior
{
    [SerializeField] Vector2[] patrolPoints;
    [SerializeField] float waitTimer = 1f;
    bool patrolActive = true;
    int goal = 0;
    

    Vector2 currentPos = Vector2.zero;
    Vector2 heading = Vector2.zero;

    Vector2 gizmosCubeSize = new Vector2(0.5f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        goal = 0;
    }

    public override void Act(Enemy e)
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
            e.motionInput.x = (heading.x > 0) ? -1 : 1;
        } else {
            e.motionInput.x = 0;
        }
        
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(waitTimer);
        patrolActive = true;
    }

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
}
