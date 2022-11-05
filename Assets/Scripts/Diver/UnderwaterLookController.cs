using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterLookController : MonoBehaviour
{
    public float fogDensity = .1f;
    public float fogStart = 5f;
    public float fogEnd = 35f;
    public Color underwaterColor = Color.blue;
    //public Color underwaterAmbient = Color.magenta;
    
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.backgroundColor = underwaterColor;
        RenderSettings.fog = true;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogStartDistance = fogStart;
        RenderSettings.fogEndDistance = fogEnd;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.ambientLight = underwaterColor;
        RenderSettings.ambientSkyColor = underwaterColor;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.skybox = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
