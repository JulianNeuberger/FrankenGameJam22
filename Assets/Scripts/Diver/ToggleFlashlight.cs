using UnityEngine;
using UnityEngine.UI;

public class ToggleFlashlight : MonoBehaviour
{
    public Light light;
    public Image icon;
    public Color activeColor;
    public Color inactiveColor;

    // Start is called before the first frame update
    void Start()
    {
        icon.color = activeColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetButtonDown("ToggleFlashlight"))
        {
            return;
        }

        var isNowActive = !light.gameObject.activeSelf;
        light.gameObject.SetActive(isNowActive);
        icon.color = isNowActive ? activeColor : inactiveColor;
    }
}