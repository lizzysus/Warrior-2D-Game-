using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movSpeed;
    [SerializeField] private Rigidbody2D rb;
    private float charInp;

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
    }
}
