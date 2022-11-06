using UnityEngine;

public class ShipMovementController : MonoBehaviour
{
    public GameObject captain;

    public float acceleration;
    public float maxSpeed;

    public float currentSpeed;

    private InteractionManager interactionManager;

    void Start()
    {
        if (NetworkSyncer.Get())
        {
            NetworkSyncer.Get().UpdateShipPositionServerRpc(transform.position);
        }
        interactionManager = captain.GetComponent<InteractionManager>();
    }


    private void Update()
    {
        if (!interactionManager.GetIsSteeringActive())
        {
            return;
        }

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
        if (!interactionManager.GetIsSteeringActive())
        {
            return;
        }

        //Move ship
        transform.Translate(new Vector3(-1, 0, 0) * currentSpeed * Time.deltaTime);

        if (NetworkSyncer.Get())
        {
            NetworkSyncer.Get().UpdateShipPositionServerRpc(transform.position);
        }
    }
}
