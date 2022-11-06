using UnityEngine;

public class CaptainMovementController : MonoBehaviour
{
    public bool captainMovementDeactivated = false;
    public float movementSpeed;

    public Animator animator;
    private Rigidbody2D rb;

    private Vector2 movement;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if(captainMovementDeactivated)
        {
            return;
        }

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }


    private void FixedUpdate()
    {
        if (captainMovementDeactivated)
        {
            return;
        }

        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
}
