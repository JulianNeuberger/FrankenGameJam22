using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonUseManager : MonoBehaviour
{    
    private int buttonLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        buttonLayer = LayerMask.GetMask("Buttons");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5f, buttonLayer))
        {
            var button = hit.collider.gameObject.GetComponent<UseableButton>();
            if (button != null)
            {
                button.Use();
            }
        }
    }
}