using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ActionState {IDLE, RUNNING, JUMPING, FALLING, ATTACK, DASH, NONE};
public class Player: EntityController
{
    
    [HideInInspector] public Hurtbox hurtbox;

    private Animator anim;
    private Movement movement;
    private Attack attack;
    private Dash dash;
    public Vector2 motionInput = Vector2.zero;

    private void Awake()
    {
        GameManager.Instance.Player = this;
        movement = GetComponent<Movement>();
        anim = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        hurtbox = GetComponent<Hurtbox>();
        dash = GetComponent<Dash>();
    }


    // Update is called once per frame
    void Update()
    {
        if (hurtbox.health < 0.01) Die();
        if (stateOverride) return;

        ActionState newState = ActionState.NONE;
        if (Input.GetButtonDown("Attack")) {
            stateOverride = true;
            attack.Act();
        }


        if (newState == ActionState.NONE) {
            motionInput.x = Input.GetAxis("Horizontal"); // Just found out that this is not great for two inputs at a time
            // TODO: Remake movement system to be smoother than getAxis
            motionInput.y = Input.GetButtonDown("Jump") ? 1 : 0;

            if (motionInput.x != 0 && Input.GetButtonDown("Dash")) {
                stateOverride = true;
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

