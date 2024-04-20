using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    public float moveSpeed = 10;
    public float jumpHeight = 4.5f;
    public int jumpLimit = 1;

    [HideInInspector] public int jumpCount {get; private set;} = 0;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    [SerializeField] bool InvertFlipX = false; // this may need to be put into the enemy script, and made into a method
    private float jumpForce;
    private Vector2 motionVector = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public ActionState Move(Vector2 motionInput)
    {
        ActionState motionState = ActionState.IDLE;
        float horizontalInput = motionInput.x;

        // anything but 0 implies player input
        if (horizontalInput != 0)
        {
            if (horizontalInput < 0) sprite.flipX = !InvertFlipX; 
            else if (horizontalInput > 0) sprite.flipX = InvertFlipX;

            motionVector.x = horizontalInput * moveSpeed;
            motionState = ActionState.RUNNING;
        } else {
            motionVector.x = rb.velocity.x;
        }

        // check for number of jumps allowed, allows us to potentially add double jumps
        if (motionInput.y == 1 && jumpCount < jumpLimit)
        {
            float jumpForce = Mathf.Sqrt(2 * jumpHeight
            * -(Physics2D.gravity.y * rb.gravityScale)) 
            * rb.mass;

            motionVector.y = jumpForce;
            jumpCount++;
        } else {
            motionVector.y = rb.velocity.y;
        }
        
        rb.velocity = motionVector;

        // comparing to y velocity state change
        if (rb.velocity.y < -0.01) 
        {
            motionState = ActionState.FALLING;
        } 
        else if (rb.velocity.y > 0.01 && jumpCount > 0) 
        {
            motionState = ActionState.JUMPING;
        } else {
            
        }


        return motionState;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            jumpCount = 0;
        }
    }

}
