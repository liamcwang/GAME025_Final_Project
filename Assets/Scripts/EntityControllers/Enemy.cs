using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityController
{
    [HideInInspector] public List<AIBehavior> continuousBehaviors = new List<AIBehavior>();
    private Animator anim;
    private Movement movement;
    private SpriteRenderer sprite;

    private void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        var behaviors = GetComponents<AIBehavior>();
        foreach (var b in behaviors) {
            switch (b.behaviorType)
            {
                case BehaviorType.ON_START:
                    b.Act();
                    break;
                case BehaviorType.CONTINUOUS:
                    continuousBehaviors.Add(b);
                    break;
                default:
                    break;
            }
        sprite = GetComponent<SpriteRenderer>();
        //StartCoroutine(Patrol());
        }
    }

    private void Update()
    {
        foreach (AIBehavior b in continuousBehaviors) {
            b.Act();
        }
        state = movement.Move(motionInput);

        if (anim != null) {
            anim.SetInteger("ActionState", (int) state);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        Player p = other.gameObject.GetComponent<Player>();
        if (p != null) {
            Destroy(gameObject);
            GameManager.WinGame();
        }    
    }*/

    /*
    IEnumerator Patrol() {
        while(true) {
            yield return new WaitForSeconds(1f);
            Debug.Log("Yep");
            if (transform.position.x > 0)
            motionInput.x = rightLeftSwitch ? 1 : -1;

        }   
    }*/
}
