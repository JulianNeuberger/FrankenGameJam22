using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementController : MonoBehaviour
{
    public bool shipMovementDeactivated = true;
    public float acceleration;
    public float maxSpeed;

    public float currentSpeed;


    void Start()
    {
    }


    private void Update()
    {
        if (shipMovementDeactivated)
        {
            return;
        }

        var v = Input.GetAxisRaw("Vertical");

        if (v > 0 && currentSpeed < maxSpeed)
        {
            var newCurrentSpeed = currentSpeed + (acceleration * Time.deltaTime * v);
            if(newCurrentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
            else
            {
                currentSpeed = newCurrentSpeed;
            }
        }
        else if (v < 0 && currentSpeed > 0)
        {
            var newCurrentSpeed = currentSpeed - (acceleration * Time.deltaTime * v);
            if(newCurrentSpeed < 0)
            {
                currentSpeed = 0;
            }
            else
            {
                currentSpeed = newCurrentSpeed;
            }
        }

        //TODO: MOVE SHIP


    }
}
