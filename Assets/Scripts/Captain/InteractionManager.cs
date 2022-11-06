using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject radarUi;
    public GameObject radarAvailableNotification;
    public GameObject radarActiveNotification;
    public GameObject radarArea;
    private Collider2D radarAreaCollider;
    private bool isRadarAvailable = false;
    private bool isRadarActive = false;

    public GameObject cableArea;
    public GameObject cableAvailableNotification;
    private Collider2D cableAreaCollider;
    private bool isCableAvailable = false;
    public float cableSpeed = 1f;
    private float diverTargetHeightDelta = 0f;
    private float lastTargetHeightDeltaUpdate = 0f;

    public GameObject steeringArea;
    public GameObject steeringAvailableNotification;
    public GameObject steeringActiveNotification;
    private Collider2D steeringAreaCollider;
    private bool isSteeringAvailable = false;
    private bool isSteeringActive = false;

    public GameObject ship;
    public GameObject captain;

    // Start is called before the first frame update
    void Start()
    {
        radarAreaCollider = radarArea.GetComponent<Collider2D>();
        cableAreaCollider = cableArea.GetComponent<Collider2D>();
        steeringAreaCollider = steeringArea.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("InteractWithBoat"))
        {
            if(isRadarAvailable)
            {
                ActivateRadar();
            }
            else if(isSteeringAvailable)
            {
                ActivateSteering();
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if(isRadarActive)
            {
                DeactivateRadar();
            }
            if(isSteeringActive)
            {
                DeactivateSteering();
            }
        }

        if(Input.GetButton("CableUp"))
        {
            if(isCableAvailable)
            {
                //increase diver height
                diverTargetHeightDelta += Time.deltaTime * cableSpeed;
                SendTargetHeightDeltaUpdate();
            }
        }
        if (Input.GetButton("CableDown"))
        {
            if (isCableAvailable)
            {
                //decrease diver height
                diverTargetHeightDelta -= Time.deltaTime * cableSpeed;
                SendTargetHeightDeltaUpdate();
            }
        }

        //TODO: REMOVE, ONLY FOR DEBUG

        if (Input.GetButtonDown("GiveUp"))
        {
            NetworkSyncer.Get().SetGameToLostServerRpc();
        }
        if (Input.GetButtonDown("Submit"))
        {
            NetworkSyncer.Get().SetGameToWonServerRpc();
        }
        if (Input.GetButtonDown("CollectTreasure"))
        {
            NetworkSyncer.Get().CollectTreasureServerRpc(new Vector3());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGER ENTERED!");

        if(collision == radarAreaCollider)
        {
            Debug.Log("Now in radar area");
            isRadarAvailable = true;
            if(!isRadarActive)
            {
                radarAvailableNotification.SetActive(true);
            }
        }
        if (collision == cableAreaCollider)
        {
            Debug.Log("Now in cable area");
            isCableAvailable = true;
            cableAvailableNotification.SetActive(true);
        }
        if (collision == steeringAreaCollider)
        {
            Debug.Log("Now in steering area");
            isSteeringAvailable = true;
            if(!isSteeringActive)
            {
                steeringAvailableNotification.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("TRIGGER EXITED!");

        if (collision == radarAreaCollider)
        {
            Debug.Log("Now no longer in radar area");
            isRadarAvailable = false;
            radarAvailableNotification.SetActive(false);
            if (isRadarActive)
            {
                DeactivateRadar();
            }
        }
        if (collision == cableAreaCollider)
        {
            Debug.Log("Now no longer in cable area");
            isCableAvailable = false;
            cableAvailableNotification.SetActive(false);
        }
        if (collision == steeringAreaCollider)
        {
            Debug.Log("Now no longer in steering area");
            isSteeringAvailable = false;
            steeringAvailableNotification.SetActive(false);
            if (isSteeringActive)
            {
                DeactivateSteering();
            }
        }
    }

    private void ActivateRadar()
    {
        radarAvailableNotification.SetActive(false);
        radarUi.SetActive(true);
        isRadarActive = true;
        radarActiveNotification.SetActive(true);
    }

    private void DeactivateRadar()
    {
        radarUi.SetActive(false);
        isRadarActive = false;
        radarActiveNotification.SetActive(false);
        if(isRadarAvailable)
        {
            radarAvailableNotification.SetActive(true);
        }
    }

    private void SendTargetHeightDeltaUpdate()
    {
        float timeSinceLastUpdate = Time.time - lastTargetHeightDeltaUpdate;
        Debug.Log($"TimeSinceLastUpdate: {timeSinceLastUpdate}");
        if (timeSinceLastUpdate > 0.5)
        {
            float diverTargetHeight = NetworkSyncer.Get().diverTargetHeight.Value;
            var newDiverTargetHeight = diverTargetHeight + diverTargetHeightDelta;
            NetworkSyncer.Get().UpdateDiverTargetHeightServerRpc(newDiverTargetHeight);
            lastTargetHeightDeltaUpdate = Time.time;
            diverTargetHeightDelta = 0f;
        }
    }

    private void ActivateSteering()
    {
        steeringAvailableNotification.SetActive(false);
        isSteeringActive = true;
        captain.GetComponent<CaptainMovementController>().captainMovementDeactivated = true;
        ship.GetComponent<ShipMovementController>().shipMovementDeactivated = false;
        steeringActiveNotification.SetActive(true);
    }


    private void DeactivateSteering()
    {
        isSteeringActive = false;
        captain.GetComponent<CaptainMovementController>().captainMovementDeactivated = false;
        ship.GetComponent<ShipMovementController>().shipMovementDeactivated = true;
        steeringActiveNotification.SetActive(false);
        if(isSteeringAvailable)
        {
            steeringAvailableNotification.SetActive(true);
        }
    }
}
