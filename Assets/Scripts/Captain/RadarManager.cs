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

    void Start()
    {
        
    }


    void Update()
    {
        //=== update diverRadarMarker position ===
        var oldDiverRadarMarkerPosition = diverRadarMarker.transform.position;

        //default
        float diverRadarX = 0;
        float diverRadarY = 0;

        //map diverPosition in 3D to diverRadarPosition in 2D (mapping 3D z-axis to 2D y-axis)
        if (NetworkSyncer.Get())
        {
            Vector3 diverPosition = NetworkSyncer.Get().diverPosition.Value;

            diverRadarX = diverPosition.x / diverPositionMaxX * radarMaxX;
            diverRadarY = diverPosition.z / diverPositionMaxZ * radarMaxY;
        }

        diverRadarMarker.transform.position = new Vector3(diverRadarX, diverRadarY, oldDiverRadarMarkerPosition.z);
    }
}
