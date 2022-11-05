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
        NetworkSyncer.Get().UpdateShipPositionServerRpc(transform.position);
    }


    private void Update()
    {
        if (shipMovementDeactivated)
        {
            Debug.Log("Shipmovement is deactivated");
            return;
        }

        Debug.Log("Shipmovement is ACTIVE");


        //Compute speed
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
            var newCurrentSpeed = currentSpeed + (acceleration * Time.deltaTime * v);
            if(newCurrentSpeed < 0)
            {
                currentSpeed = 0;
            }
            else
            {
                currentSpeed = newCurrentSpeed;
            }
        }

    }

    private void FixedUpdate()
    {
        //Move ship
        transform.Translate(new Vector3(-1, 0, 0) * currentSpeed * Time.deltaTime);

        if (NetworkSyncer.Get())
        {
            NetworkSyncer.Get().UpdateShipPositionServerRpc(transform.position);
        }
    }
}
