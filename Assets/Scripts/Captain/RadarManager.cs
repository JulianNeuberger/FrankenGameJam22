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

    private Vector3 simulatedShipPosition =  Vector3.zero;

    void Start()
    {
        existingTreasureRadarMarkers = new Dictionary<Vector3, GameObject>();
    }


    void Update()
    {
        if (NetworkSyncer.Get())
        {
            simulatedShipPosition = NetworkSyncer.Get().shipPosition.Value;
        }

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

            Vector2 diverRadarPosition = MapRealPositionToRadarPosition(diverPosition);

            //restrict to radar size on screen (showing at the edge if outside)
            var absoluteDiverRadarX = Mathf.Clamp(diverRadarPosition.x, -screenRadarMaxX, screenRadarMaxX);
            var absoluteDiverRadarY = Mathf.Clamp(diverRadarPosition.y, -screenRadarMaxY, screenRadarMaxY);

            //set relative to center of the radar to compensate radar placement in worldspace
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

            Vector2 sharkRadarPosition = MapRealPositionToRadarPosition(sharkPosition);

            //restrict to radar size on screen (not showing if outside)
            if (sharkRadarPosition.x > screenRadarMaxX ||
                sharkRadarPosition.y > screenRadarMaxY ||
                sharkRadarPosition.x < -screenRadarMaxX ||
                sharkRadarPosition.y < -screenRadarMaxY)
            {
                sharkRadarMarker.SetActive(false);
            }
            else
            {
                //set relative to center of the radar to compensate radar placement in worldspace
                var relativeSharkRadarX = centerMarker.transform.position.x + sharkRadarPosition.x;
                var relativeSharkRadarY = centerMarker.transform.position.y + sharkRadarPosition.y;

                sharkRadarMarker.transform.position = new Vector3(relativeSharkRadarX, relativeSharkRadarY, sharkRadarMarker.transform.position.z);
            }

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
                if(existingTreasureRadarMarkers.ContainsKey(newTreasurePosition))
                {
                    UpdateTreasureMarker(newTreasurePosition);
                }
                else                        
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
        Debug.Log($"Adding treasure marker {treasurePosition}");
        Vector2 treasureRadarPosition = MapRealPositionToRadarPosition(treasurePosition);

        //restrict to radar size on screen (showing at the edge if outside)
        var absoluteTreasureRadarX = Mathf.Clamp(treasureRadarPosition.x, -screenRadarMaxX, screenRadarMaxX);
        var absoluteTreasureRadarY = Mathf.Clamp(treasureRadarPosition.y, -screenRadarMaxY, screenRadarMaxY);

        //set relative to center of the radar to compensate radar placement in worldspace
        var relativeTreasureRadarX = centerMarker.transform.position.x + absoluteTreasureRadarX;
        var relativeTreasureRadarY = centerMarker.transform.position.y + absoluteTreasureRadarY;

        var relativeTreasureRadarPosition = new Vector3(relativeTreasureRadarX, relativeTreasureRadarY, diverRadarMarker.transform.position.z);
        
        var treasureRadarMarker = Instantiate(treasureMarkerPrefab, relativeTreasureRadarPosition, Quaternion.identity, transform);

        existingTreasureRadarMarkers.Add(treasurePosition, treasureRadarMarker);
    }

    void UpdateTreasureMarker(Vector3 treasurePosition)
    {
        Debug.Log($"Updating treasure marker {treasurePosition}");
        var existingMarker = existingTreasureRadarMarkers[treasurePosition];

        Vector2 treasureRadarPosition = MapRealPositionToRadarPosition(treasurePosition);

        //restrict to radar size on screen (showing at the edge if outside)
        var absoluteTreasureRadarX = Mathf.Clamp(treasureRadarPosition.x, -screenRadarMaxX, screenRadarMaxX);
        var absoluteTreasureRadarY = Mathf.Clamp(treasureRadarPosition.y, -screenRadarMaxY, screenRadarMaxY);

        //set relative to center of the radar to compensate radar placement in worldspace
        var relativeTreasureRadarX = centerMarker.transform.position.x + absoluteTreasureRadarX;
        var relativeTreasureRadarY = centerMarker.transform.position.y + absoluteTreasureRadarY;

        var relativeTreasureRadarPosition = new Vector3(relativeTreasureRadarX, relativeTreasureRadarY, diverRadarMarker.transform.position.z);

        existingMarker.transform.position = relativeTreasureRadarPosition;
    }



    private Vector2 MapRealPositionToRadarPosition(Vector3 realPosition)
    {
        //compute real world position relative to ship position
        var relativeX = realPosition.x - simulatedShipPosition.x;
        var relativeZ = realPosition.z - simulatedShipPosition.z;

        //map 3D world to 2D radar (mapping 3D z-axis to 2D y-axis and scaling to radar screen)
        float radarX = relativeX / realWorldMaxRadarX * screenRadarMaxX;
        float radarY = relativeZ / realWorldMaxRadarZ * screenRadarMaxY;

        return new Vector2(radarX, radarY);
    }
}
