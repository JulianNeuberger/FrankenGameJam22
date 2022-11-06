using UnityEngine;

public class ShipMovementController : MonoBehaviour
{
    public GameObject captain;
    public Animator shipWaveAnimator;

    public float acceleration;
    public float maxSpeed;

    public float currentSpeed;

    private Vector3 simulatedShipPositon = new Vector3(500f, 0f, 500f); //hardcoded to approximately match diver start position

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

        shipWaveAnimator.SetFloat("Speed", currentSpeed);

    }

    private void FixedUpdate()
    {
        //Simulate ship movement
        simulatedShipPositon += new Vector3(-1, 0, 0) * currentSpeed * Time.deltaTime;

        if (NetworkSyncer.Get())
        {
            NetworkSyncer.Get().UpdateShipPositionServerRpc(simulatedShipPositon);
        }
    }
}
