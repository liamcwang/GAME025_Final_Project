using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ActionState {IDLE, RUNNING, JUMPING, FALLING, ATTACK, NONE};
public class Player: MonoBehaviour
{
    
    public ActionState state;
    public bool stateOverride;
    [HideInInspector] public Hurtbox hurtbox;

    private Animator anim;
    private Movement movement;
    private Attack attack;
    private Vector2 motionInput = Vector2.zero;

    private void Awake()
    {
        GameManager.Player = this;
        movement = GetComponent<Movement>();
        anim = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        hurtbox = GetComponent<Hurtbox>();
        hurtbox.onTakeDamage += HealthChanged;
    }

    private void Start()
    {
        HealthChanged();
    }

    // Update is called once per frame
    void Update()
    {
        if (hurtbox.health < 0.01) Die();
        if (stateOverride) return;

        ActionState newState = ActionState.NONE;
        if (Input.GetButtonDown("Attack")) {
            attack.Act();
            stateOverride = true;
        }

        if (newState == ActionState.NONE) {
            motionInput.x = Input.GetAxis("Horizontal");
            motionInput.y = Input.GetButtonDown("Jump") ? 1 : 0;
            newState = movement.Move(motionInput);
            
        }

        if (newState != ActionState.NONE) state = newState;

        anim.SetInteger("PlayerState", (int) state);   
        
    }

    public void HealthChanged() {
        float healthRatio = hurtbox.health / hurtbox.maxHealth;
        GameManager.Canvas.UpdateHealth(healthRatio);
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
        GameManager.Defeat();
    }
}

