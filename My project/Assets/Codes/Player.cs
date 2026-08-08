using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movSpeed;
    private float charInp;
    [SerializeField] private float jump;
    private bool isFacingRight;
    private bool isGrounded;
    private bool canMove = true;
    private bool canJump = true;

    [Header("Collision & Component")]
    [SerializeField] private Rigidbody2D rb;
    private Animator anim;
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    private void HandleMovement() 
    {
        charInp = (Input.GetAxisRaw("Horizontal"));

        if(canMove)
        rb.linearVelocity = new Vector2(charInp * movSpeed, rb.linearVelocity.y);
        else
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if(charInp > 0 && isFacingRight ) 
        {
            Flip();
        }
        else if (charInp < 0 && !isFacingRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) 
        {
            Jump();
        } 
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded) 
        {
            rb.linearVelocity = new Vector2(charInp * movSpeed, rb.linearVelocity.y);
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            AttemptToAttack();
        }
         
    }

    private void Jump() 
    {
        if(isGrounded && canJump)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
    }

    private void AttemptToAttack() 
    {
        if (isGrounded)
        {
            anim.SetTrigger("isAttacking");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        }
    }

    private void Flip() 
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;

    }

    private void OnCollisionEnter2D(Collision2D groundcheck)
    {
        if (groundcheck.gameObject.layer == LayerMask.NameToLayer("platf_")) 
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D groundcheck)
    {
        if(groundcheck.gameObject.layer == LayerMask.NameToLayer("platf_")) 
        {
            isGrounded = false;
        }
    }

    private void HandleAnimation() 
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public void EnableJumpMovement(bool enable)  
    {
        canMove = enable;
        canJump = enable;
    }


}
