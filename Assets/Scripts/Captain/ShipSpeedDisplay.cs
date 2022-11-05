using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipSpeedDisplay : MonoBehaviour
{
    public GameObject ship;
    private TextMeshProUGUI label;
    private ShipMovementController shipMovementController;
    
    // Start is called before the first frame update
    void Start()
    {
        label = GetComponent<TextMeshProUGUI>();
        shipMovementController = ship.GetComponent<ShipMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        label.text = shipMovementController.currentSpeed.ToString();
    }
}
