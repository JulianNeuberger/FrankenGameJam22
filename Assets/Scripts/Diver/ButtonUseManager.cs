using UnityEngine;

public class ButtonUseManager : MonoBehaviour
{
    public AudioSource buttonSound;
    
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
                buttonSound.Play();
                button.Use();
            }
        }
    }
}