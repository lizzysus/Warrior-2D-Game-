using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movSpeed;
    private float charInp;
    [SerializeField] private float jump;
    private bool isFacingRight;

    [Header("Collision & Component")]
    [SerializeField] private Rigidbody2D rb;
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement() 
    {
        charInp = (Input.GetAxisRaw("Horizontal"));

        rb.linearVelocity = new Vector2(charInp * movSpeed, rb.linearVelocity.y);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if(charInp > 0 && isFacingRight) 
        {
            Flip();
        }
        else if (charInp < 0 && !isFacingRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Jump();
        }
    }

    private void Jump() 
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
    }

    private void Flip() 
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;

    }
}
