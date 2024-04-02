using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ActionState {IDLE, RUNNING, JUMPING, FALLING, ATTACK, DASH, NONE};
public class Player: EntityController
{
    
    [HideInInspector] public Hurtbox hurtbox;
    public float sensitivity = 3;

    private Animator anim;
    private Movement movement;
    private Attack attack;
    private Dash dash;
    private SpriteRenderer sprite;

    private void Awake()
    {
        GameManager.Instance.Player = this;
        movement = GetComponent<Movement>();
        anim = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        hurtbox = GetComponent<Hurtbox>();
        dash = GetComponent<Dash>();
        sprite = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (hurtbox.health < 0.01) Die();
        if (stateOverride) return;

        ActionState newState = ActionState.NONE;
        if (Input.GetButtonDown("Attack")) {
            attack.Act();
        }


        if (newState == ActionState.NONE) {
            // This solution works better than GetAxis("Horizontal") believe it or not.
            motionInput.x = Input.GetAxis("Left") + Input.GetAxis("Right");
            motionInput.y = Input.GetButtonDown("Jump") ? 1 : 0;

            if (motionInput.x != 0 && Input.GetButtonDown("Dash")) {
                dash.Act(motionInput.x);
            } else {
                newState = movement.Move(motionInput);
            }
            
        }

        if (newState != ActionState.NONE) state = newState;

        anim.SetInteger("PlayerState", (int) state);   
        
    }

    public void Die() {
        StartCoroutine(DeathSequence());
    }

    public void loseControl(float disableTimer) {
        stateOverride = true;
        anim.Play("hurt");
        StartCoroutine(RegainControl(disableTimer));
    }

    IEnumerator RegainControl(float timer) {
        yield return new WaitForSeconds(timer);
        stateOverride = false;
    }

    IEnumerator DeathSequence() {
        anim.Play("death");
        yield return new WaitForSeconds(2f);
        GameManager.RestartGame();
    }
}

