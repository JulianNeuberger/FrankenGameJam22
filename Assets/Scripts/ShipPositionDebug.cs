using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipPositionDebug : MonoBehaviour
{
    private TextMeshProUGUI label;
    
    // Start is called before the first frame update
    void Start()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(NetworkSyncer.Get())
        {
            label.text = NetworkSyncer.Get().shipPosition.Value.ToString();
        }
    }
}
