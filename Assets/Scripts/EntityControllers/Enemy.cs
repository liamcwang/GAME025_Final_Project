using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityController
{
    [HideInInspector] public AIBehavior[] behaviors;

    private Animator anim;
    private Movement movement;
    private SpriteRenderer sprite;

    private void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        behaviors = GetComponents<AIBehavior>();
        sprite = GetComponent<SpriteRenderer>();
        //StartCoroutine(Patrol());
    }

    private void Update()
    {
        foreach (AIBehavior b in behaviors) {
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
