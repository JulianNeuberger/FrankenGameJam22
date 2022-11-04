using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PositionDebug : MonoBehaviour
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
        label.text = NetworkSyncer.Get().diverPosition.Value.ToString();
    }
}
