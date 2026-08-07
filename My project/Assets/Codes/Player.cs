using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float charInp;
    [SerializeField] private float movSpeed;

    private void Update()
    {

    }

    private void HandleMovement() 
    {
        charInp = (Input.GetAxisRaw("Horizontal"));

    }
}
