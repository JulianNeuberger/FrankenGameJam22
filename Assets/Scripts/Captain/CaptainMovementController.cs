using UnityEngine;

public class CaptainMovementController : MonoBehaviour
{
    public bool captainMovementDeactivated = false;
    public float movementSpeed;

    public Animator animator;
    private Rigidbody2D rb;

    private Vector2 movement;

    private InteractionManager interactionManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        interactionManager = GetComponent<InteractionManager>();
    }


    void Update()
    {
        if (interactionManager.GetIsSteeringActive() || interactionManager.GetIsRadarActive())
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
        if (interactionManager.GetIsSteeringActive() || interactionManager.GetIsRadarActive())
        {
            return;
        }

        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
}
