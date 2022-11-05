using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed;
    public float turnSpeed;
    private float horizontalInput;
    private float verticalInput;

    private Rigidbody2D rb2D;

    private Vector2 moveDirection;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //horizontalInput = Input.GetAxis("Horizontal");
        ////rb2D.MovePosition()
        //transform.Rotate(Vector3.back, Time.deltaTime * turnSpeed * horizontalInput);


        //verticalInput = Input.GetAxis("Vertical");
        //rb2D.MovePosition(rb2D.position + new Vector2(1, 0) * Time.deltaTime);
        ////transform.Translate(Vector3.right * Time.deltaTime * movementSpeed * verticalInput);

        ProcessInputs();
    }


    private void FixedUpdate()
    {
        Move();
        RotateTowardDirection();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        moveDirection = new Vector2(moveX, moveY);

    }

    void Move()
    {
        rb2D.velocity = moveDirection * movementSpeed;
    }

    public void RotateTowardDirection()
    {
        if (moveDirection != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, moveDirection);
        }
    }
}
