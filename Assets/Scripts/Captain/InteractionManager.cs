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

    public GameObject bookUi;
    public GameObject bookAvailableNotification;
    public GameObject bookActiveNotification;
    public GameObject bookArea;
    private Collider2D bookAreaCollider;
    private bool isBookAvailable = false;
    private bool isBookActive = false;


    // Start is called before the first frame update
    void Start()
    {
        radarAreaCollider = radarArea.GetComponent<Collider2D>();
        cableAreaCollider = cableArea.GetComponent<Collider2D>();
        steeringAreaCollider = steeringArea.GetComponent<Collider2D>();
        bookAreaCollider = bookArea.GetComponent<Collider2D>();
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
            else if (isBookAvailable)
            {
                ActivateBook();
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
            if (isBookActive)
            {
                DeactivateBook();
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
        //Debug.Log("TRIGGER ENTERED!");

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
        if (collision == bookAreaCollider)
        {
            Debug.Log("Now in book area");
            isBookAvailable = true;
            if (!isBookActive)
            {
                bookAvailableNotification.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("TRIGGER EXITED!");

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
        if (collision == bookAreaCollider)
        {
            Debug.Log("Now no longer in book area");
            isBookAvailable = false;
            bookAvailableNotification.SetActive(false);
            if (isBookActive)
            {
                DeactivateRadar();
            }
        }
    }

    private void ActivateRadar()
    {
        Debug.Log("Activating Radar");
        radarAvailableNotification.SetActive(false);
        radarUi.SetActive(true);
        isRadarActive = true;
        radarActiveNotification.SetActive(true);
    }

    private void DeactivateRadar()
    {
        Debug.Log("Deactivating Radar");
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
        Debug.Log("Send target height delta update");
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
        Debug.Log("Activating Steering");
        steeringAvailableNotification.SetActive(false);
        isSteeringActive = true;
        steeringActiveNotification.SetActive(true);
    }


    private void DeactivateSteering()
    {
        Debug.Log("Deactivating Steering");
        isSteeringActive = false;
        steeringActiveNotification.SetActive(false);
        if(isSteeringAvailable)
        {
            steeringAvailableNotification.SetActive(true);
        }
    }

    private void ActivateBook()
    {
        Debug.Log("Activating Book");
        bookAvailableNotification.SetActive(false);
        bookUi.SetActive(true);
        isBookActive = true;
        bookActiveNotification.SetActive(true);
    }

    private void DeactivateBook()
    {
        Debug.Log("Deactivating Book");
        bookUi.SetActive(false);
        isBookActive = false;
        bookActiveNotification.SetActive(false);
        if (isBookAvailable)
        {
            bookAvailableNotification.SetActive(true);
        }
    }


    public bool GetIsSteeringActive()
    {
        return isSteeringActive;
    }

    public bool GetIsRadarActive()
    {
        return isRadarActive;
    }

    public bool GetIsBookActive()
    {
        return isRadarActive;
    }
}
