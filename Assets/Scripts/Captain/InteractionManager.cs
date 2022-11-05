using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject radarUi;
    public GameObject radarNotification;
    public GameObject radarArea;
    private Collider2D radarAreaCollider;
    private bool isRadarAvailable = false;
    private bool isRadarActive = false;


    public GameObject cableArea;
    public GameObject cableNotification;
    private Collider2D cableAreaCollider;
    private bool isCableAvailable = false;
    public float cableSpeed = 1f;

    public GameObject exitNotification;


    private float diverTargetHeightDelta = 0f;
    private float lastTargetHeightDeltaUpdate = 0f;


    // Start is called before the first frame update
    void Start()
    {
        radarAreaCollider = radarArea.GetComponent<Collider2D>();
        cableAreaCollider = cableArea.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(isRadarAvailable)
            {
                ActivateRadar();
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if(isRadarActive)
            {
                DeactivateRadar();
            }
        }

        if(Input.GetButton("Fire1"))
        {
            if(isCableAvailable)
            {
                //increase diver height
                diverTargetHeightDelta += Time.deltaTime * cableSpeed;
                SendTargetHeightDeltaUpdate();
            }
        }
        if (Input.GetButton("Fire2"))
        {
            if (isCableAvailable)
            {
                //decrease diver height
                diverTargetHeightDelta -= Time.deltaTime * cableSpeed;
                SendTargetHeightDeltaUpdate();
            }
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
                radarNotification.SetActive(true);
            }
        }
        if (collision == cableAreaCollider)
        {
            Debug.Log("Now in cable area");
            isCableAvailable = true;
            cableNotification.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("TRIGGER EXITED!");

        if (collision == radarAreaCollider)
        {
            Debug.Log("Now no longer in radar area");
            isRadarAvailable = false;
            radarNotification.SetActive(false);
        }
        if (collision == cableAreaCollider)
        {
            Debug.Log("Now no longer in cable area");
            isCableAvailable = false;
            cableNotification.SetActive(false);
        }
    }

    private void ActivateRadar()
    {
        radarNotification.SetActive(false);
        radarUi.SetActive(true);
        isRadarActive = true;
        exitNotification.SetActive(true);
    }

    private void DeactivateRadar()
    {
        radarUi.SetActive(false);
        isRadarActive = false;
        exitNotification.SetActive(false);

        if(isRadarAvailable)
        {
            radarNotification.SetActive(true);
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
}
