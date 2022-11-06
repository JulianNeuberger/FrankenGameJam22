using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    public float diverPositionMaxX;
    public float diverPositionMaxZ;

    public float radarMaxX;
    public float radarMaxY;

    public GameObject diverRadarMarker;
    public GameObject centerMarker;

    void Start()
    {
        
    }


    void Update()
    {
        //default
        float absoluteDiverRadarX = 0;
        float absoluteDiverRadarY = 0;

        //map diverPosition in 3D to diverRadarPosition in 2D (mapping 3D z-axis to 2D y-axis)
        if (NetworkSyncer.Get())
        {
            Vector3 diverPosition = NetworkSyncer.Get().diverPosition.Value;

            absoluteDiverRadarX = diverPosition.x / diverPositionMaxX * radarMaxX;
            absoluteDiverRadarY = diverPosition.z / diverPositionMaxZ * radarMaxY;

            if(absoluteDiverRadarX > radarMaxX)
            {
                absoluteDiverRadarX = radarMaxX;
            }
            if(absoluteDiverRadarY > radarMaxY)
            {
                absoluteDiverRadarY = radarMaxY;
            }
        }

        var relativeDiverRadarX = centerMarker.transform.position.x + absoluteDiverRadarX;
        var relativeDiverRadarY = centerMarker.transform.position.y + absoluteDiverRadarY;

        diverRadarMarker.transform.position = new Vector3(relativeDiverRadarX, relativeDiverRadarY, diverRadarMarker.transform.position.z);
    }
}
