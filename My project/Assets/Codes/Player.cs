using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")] //header for movement related variables
    [SerializeField] private float movSpeed; //float type data named movSpeed that stores the movement speed of player
    private float charInp; //float type data that stores the player's horizontal input (-1, 0, or 1)
    [SerializeField] private float jump; //float type data that stores how much upward force is applied when jumping
    private bool isFacingRight; //bool that tracks which direction the character is currently facing
    private bool isGrounded; //bool that tracks whether the character is touching the ground
    private bool canMove = true; //bool that lets us disable movement from other scripts (e.g. during cutscenes)
    private bool canJump = true; //bool that lets us disable jumping from other scripts

    [Header("Collision & Component")]
    [SerializeField] private Rigidbody2D rb; //reference to the Rigidbody2D component that handles physics
    private Animator anim; //reference to the Animator component that controls animations
    

    [UnitHeaderInspectable("Attack Details")]
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform attackPoint; //float that determines how far the attack can reach
    [SerializeField] private LayerMask Enemy;
    private void Awake()
    {
        //Awake runs once when the object is created, before Start or Update
        //we use it to grab references to components attached to this GameObject
        rb = GetComponent<Rigidbody2D>(); //gets the Rigidbody2D attached to this same GameObject
        anim = GetComponentInChildren<Animator>(); //gets the Animator from a child GameObject (e.g. the sprite)
    }

    private void Update()
    {
        //Update runs every frame, so we call our movement and animation logic here
        HandleMovement();
        HandleAnimation();
    }

    public void DamageEnemies() 
    {
        //array to hold multiple colliders if needed (e.g. for attacks)
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, Enemy);
        Debug.Log("-10 hp");
    }

    private void HandleMovement()
    {
        //GetAxisRaw returns -1, 0, or 1 depending on A/D or Left/Right arrow keys
        charInp = (Input.GetAxisRaw("Horizontal"));

        //if movement is allowed, apply horizontal speed based on input
        //we keep the existing y velocity so gravity/jumping still works
        if (canMove)
            rb.linearVelocity = new Vector2(charInp * movSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); //if movement is disabled, stop horizontal movement

        //locks rotation so the player doesn't tip over from physics collisions
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //flip the character sprite when input direction doesn't match the way they're currently facing
        if (charInp > 0 && isFacingRight)
        {
            Flip();
        }
        else if (charInp < 0 && !isFacingRight)
        {
            Flip();
        }

        //jump only if space is pressed this frame AND the player is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            //if space is pressed while in the air, this just re-applies horizontal movement
            //(note: this doesn't actually jump since isGrounded is false)
            rb.linearVelocity = new Vector2(charInp * movSpeed, rb.linearVelocity.y);
        }

        //left mouse click triggers an attack attempt
        if (Input.GetMouseButtonDown(0))
        {
            AttemptToAttack();
        }

    }

    private void Jump()
    {
        //only jump if grounded and jumping is currently allowed
        //we keep the current x velocity so horizontal movement isn't interrupted
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
    }

    private void AttemptToAttack()
    {
        //only allow attacking while grounded
        if (isGrounded)
        {
            anim.SetTrigger("isAttacking"); //tells the Animator to play the attack animation
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); //stop horizontal movement while attacking

        }
    }

    private void Flip()
    {
        //rotating 180 degrees on the y-axis visually flips the sprite left/right
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight; //update our tracking bool to match the new direction

    }

    private void OnCollisionEnter2D(Collision2D groundcheck)
    {
        //called automatically when this object's collider touches another collider
        //checks if what we collided with is on the "platf_" (platform) layer
        if (groundcheck.gameObject.layer == LayerMask.NameToLayer("platf_"))
        {
            isGrounded = true; //we've landed on the ground
        }
    }

    private void OnCollisionExit2D(Collision2D groundcheck)
    {
        //called automatically when this object's collider stops touching another collider
        if (groundcheck.gameObject.layer == LayerMask.NameToLayer("platf_"))
        {
            isGrounded = false; //we've left the ground
        }
    }

    private void HandleAnimation()
    {
        //passes current physics values into the Animator so it can blend animations accordingly
        anim.SetFloat("xVelocity", rb.linearVelocity.x); //used for run/idle blend
        anim.SetBool("isGrounded", isGrounded); //used to switch between grounded/airborne animations
        anim.SetFloat("yVelocity", rb.linearVelocity.y); //used for jump/fall blend
    }

    public void EnableJumpMovement(bool enable)
    {
        //public method so other scripts (like cutscene managers) can enable/disable player control
        canMove = enable;
        canJump = enable;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}