using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject radarUi;
    public GameObject radarArea;
    public GameObject radarNotification;
    private Collider2D radarAreaCollider;
    private bool isRadarAvailable = false;
    private bool isRadarActive = false;


    public GameObject exitNotification;


    // Start is called before the first frame update
    void Start()
    {
        radarAreaCollider = radarArea.GetComponent<Collider2D>();
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
}
