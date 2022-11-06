using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    public float realWorldMaxRadarX;
    public float realWorldMaxRadarZ;

    public float screenRadarMaxX;
    public float screenRadarMaxY;

    public GameObject diverRadarMarker;
    public GameObject centerMarker;
    public GameObject sharkRadarMarker;

    public GameObject treasureMarkerPrefab;

    private Dictionary<Vector3, GameObject> existingTreasureRadarMarkers;

    void Start()
    {
        existingTreasureRadarMarkers = new Dictionary<Vector3, GameObject>();
    }


    void Update()
    {
        UpdateDiverMarker();
        UpdateSharkMarker();
        UpdateTreasureMarkers();
    }



    void UpdateDiverMarker()
    {
        if (NetworkSyncer.Get())
        {
            diverRadarMarker.SetActive(true);

            Vector3 diverPosition = NetworkSyncer.Get().diverPosition.Value;

            //map diverPosition in 3D to diverRadarPosition in 2D (mapping 3D z-axis to 2D y-axis and scaling to radar screen)
            float absoluteDiverRadarX = diverPosition.x / realWorldMaxRadarX * screenRadarMaxX;
            float absoluteDiverRadarY = diverPosition.z / realWorldMaxRadarZ * screenRadarMaxY;

            //restrict to radar size on screen (showing at the edge if outside)
            absoluteDiverRadarX = Mathf.Clamp(absoluteDiverRadarX, -screenRadarMaxX, screenRadarMaxX);
            absoluteDiverRadarY = Mathf.Clamp(absoluteDiverRadarY, -screenRadarMaxY, screenRadarMaxY);

            //set relative to center of the radar to compensate radar movement in worldspace
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

            //map sharkPosition in 3D to sharkRadarPosition in 2D (mapping 3D z-axis to 2D y-axis and scaling to radar screen)
            float absoluteSharkRadarX = sharkPosition.x / realWorldMaxRadarX * screenRadarMaxX;
            float absoluteSharkRadarY = sharkPosition.z / realWorldMaxRadarZ * screenRadarMaxY;

            //restrict to radar size on screen (not showing if outside)
            if (absoluteSharkRadarX > screenRadarMaxX || 
                absoluteSharkRadarY > screenRadarMaxY ||
                absoluteSharkRadarX < -screenRadarMaxX ||
                absoluteSharkRadarY < -screenRadarMaxY)
            {
                sharkRadarMarker.SetActive(false);
            }

            //set relative to center of the radar to compensate radar movement in worldspace
            var relativeSharkRadarX = centerMarker.transform.position.x + absoluteSharkRadarX;
            var relativeSharkRadarY = centerMarker.transform.position.y + absoluteSharkRadarY;

            sharkRadarMarker.transform.position = new Vector3(relativeSharkRadarX, relativeSharkRadarY, sharkRadarMarker.transform.position.z);
        }
        else
        {
            sharkRadarMarker.SetActive(false);
        }
    }

    void UpdateTreasureMarkers()
    {
        if (NetworkSyncer.Get())
        {
            var newTreasurePositions = new List<Vector3>();
            foreach(var newTreasurePosition in NetworkSyncer.Get().treasurePositions)
            {
                if(!existingTreasureRadarMarkers.ContainsKey(newTreasurePosition))
                {
                    AddTreasureMarker(newTreasurePosition);
                }
                newTreasurePositions.Add(newTreasurePosition);
            }
            foreach(var existingTreasurePosition in existingTreasureRadarMarkers.Keys)
            {
                if(!newTreasurePositions.Contains(existingTreasurePosition))
                {
                    Destroy(existingTreasureRadarMarkers[existingTreasurePosition]);
                }
            }

        }
        else
        {
            foreach (var treasureRadarMarker in existingTreasureRadarMarkers.Values)
            {
                Destroy(treasureRadarMarker);
            }
            existingTreasureRadarMarkers.Clear();
        }
    }


    void AddTreasureMarker(Vector3 treasurePosition)
    {
        //map treasurePosition in 3D to treasureRadarPosition in 2D (mapping 3D z-axis to 2D y-axis and scaling to radar screen)
        float absoluteTreasureRadarX = treasurePosition.x / realWorldMaxRadarX * screenRadarMaxX;
        float absoluteTreasureRadarY = treasurePosition.z / realWorldMaxRadarZ * screenRadarMaxY;

        //restrict to radar size on screen (showing at the edge if outside)
        absoluteTreasureRadarX = Mathf.Clamp(absoluteTreasureRadarX, -screenRadarMaxX, screenRadarMaxX);
        absoluteTreasureRadarY = Mathf.Clamp(absoluteTreasureRadarY, -screenRadarMaxY, screenRadarMaxY);

        //set relative to center of the radar to compensate radar movement in worldspace
        var relativeTreasureRadarX = centerMarker.transform.position.x + absoluteTreasureRadarX;
        var relativeTreasureRadarY = centerMarker.transform.position.y + absoluteTreasureRadarY;

        var relativeTreasureRadarPosition = new Vector3(relativeTreasureRadarX, relativeTreasureRadarY, diverRadarMarker.transform.position.z);
        
        var treasureRadarMarker = Instantiate(treasureMarkerPrefab, relativeTreasureRadarPosition, Quaternion.identity, transform);

        existingTreasureRadarMarkers.Add(treasurePosition, treasureRadarMarker);
    }
}
