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
    public GameObject sharkRadarMarker;

    void Start()
    {
        
    }


    void Update()
    {
        UpdateDiverMarker();
        UpdateSharkMarker();
    }



    void UpdateDiverMarker()
    {
        if (NetworkSyncer.Get())
        {
            diverRadarMarker.SetActive(true);

            Vector3 diverPosition = NetworkSyncer.Get().diverPosition.Value;

            //map diverPosition in 3D to diverRadarPosition in 2D (mapping 3D z-axis to 2D y-axis)
            float absoluteDiverRadarX = diverPosition.x / diverPositionMaxX * radarMaxX;
            float absoluteDiverRadarY = diverPosition.z / diverPositionMaxZ * radarMaxY;

            if (absoluteDiverRadarX > radarMaxX)
            {
                absoluteDiverRadarX = radarMaxX;
            }
            else if (absoluteDiverRadarX < -radarMaxX)
            {
                absoluteDiverRadarX = -radarMaxX;
            }
            if (absoluteDiverRadarY > radarMaxY)
            {
                absoluteDiverRadarY = radarMaxY;
            }
            else if (absoluteDiverRadarY < - radarMaxY)
            {
                absoluteDiverRadarY = -radarMaxY;
            }

            var relativeDiverRadarX = centerMarker.transform.position.x + absoluteDiverRadarX;
            var relativeDiverRadarY = centerMarker.transform.position.y + absoluteDiverRadarY;

            diverRadarMarker.transform.position = new Vector3(relativeDiverRadarX, relativeDiverRadarY, diverRadarMarker.transform.position.z);
        }
        else
        {
            diverRadarMarker.SetActive(false);
        }

    }


    void UpdateSharkMarker()
    {
        if (NetworkSyncer.Get())
        {
            sharkRadarMarker.SetActive(true);

            Vector3 sharkPosition = NetworkSyncer.Get().sharkPosition.Value;

            Debug.Log("SharkPosition:" + sharkPosition);

            //map sharkPosition in 3D to sharkRadarPosition in 2D (mapping 3D z-axis to 2D y-axis)
            float absoluteSharkRadarX = sharkPosition.x / diverPositionMaxX * radarMaxX;
            float absoluteSharkRadarY = sharkPosition.z / diverPositionMaxZ * radarMaxY;

            if (absoluteSharkRadarX > radarMaxX || 
                absoluteSharkRadarY > radarMaxY ||
                absoluteSharkRadarX < -radarMaxX ||
                absoluteSharkRadarY < -radarMaxY)
            {
                sharkRadarMarker.SetActive(false);
            }

            var relativeSharkRadarX = centerMarker.transform.position.x + absoluteSharkRadarX;
            var relativeSharkRadarY = centerMarker.transform.position.y + absoluteSharkRadarY;

            sharkRadarMarker.transform.position = new Vector3(relativeSharkRadarX, relativeSharkRadarY, sharkRadarMarker.transform.position.z);
        }
        else
        {
            sharkRadarMarker.SetActive(true);
        }


    }
}
