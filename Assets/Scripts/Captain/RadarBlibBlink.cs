using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBlibBlink : MonoBehaviour
{

    public float blinkSpeed = 1f;

    private SpriteRenderer spriteRenderer;
    private bool isIntensifying = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color tmp = spriteRenderer.color;

        float newAlpha = tmp.a;

        if(isIntensifying)
        {
            newAlpha += blinkSpeed * Time.deltaTime;
            if(newAlpha >= 1f)
            {
                isIntensifying = false;
            }
        }
        else
        {
            newAlpha -= blinkSpeed * Time.deltaTime;
            if (newAlpha <= 0f)
            {
                isIntensifying = true;
            }
        }

        Mathf.Clamp(newAlpha, 0f, 1f);
        spriteRenderer.color = new Color(tmp.r, tmp.g, tmp.b, newAlpha);
    }
}
