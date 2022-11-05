using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AltitudeMeter : MonoBehaviour
{
    public TextMeshProUGUI label;
    public float heightOffset = -20000;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var displayedHeight = (int) (transform.position.y + heightOffset);
        var formattedHeight = displayedHeight.ToString();
        label.text = $"{formattedHeight}m";
    }
}
