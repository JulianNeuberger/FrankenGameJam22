using UnityEngine;
using UnityEngine.UIElements;

public class DiverMovementController : MonoBehaviour
{
    public float forwardSpeed = 15f;
    public float turnSpeedDegrees = 10f;
    public float diveSpeed = 1f;

    public float targetHeight = 0f;

    public float swayIntensity = .1f;
    public float swayScale = 5f;

    public float maxDistanceToShip = 50f;
    public float drawToShipSpeed = 10f;

    
    private void Start()
    {
        if (NetworkSyncer.Get())
        {
            NetworkSyncer.Get().UpdateDiverPositionServerRpc(transform.position);
        }
    }
    
    private void Update()
    {
        UpdateLateralMovement();
        UpdateHeight();
        UpdateRotation();
        FixUpright();

        if (NetworkSyncer.Get())
        {
            NetworkSyncer.Get().UpdateDiverPositionServerRpc(transform.position);
        }
    }

    private void UpdateRotation()
    {
        var rotation = new Vector3(0, turnSpeedDegrees * Time.deltaTime * Input.GetAxis("Horizontal"), 0);
        rotation.y += GetSwayFactor();
        transform.Rotate(rotation);
    }

    private void FixUpright()
    {
        transform.rotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
    }
    
    private void UpdateLateralMovement()
    {
        transform.position += transform.forward * (forwardSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
        //transform.position -= transform.right * GetSwayFactor();

        if (!NetworkSyncer.Get())
        {
            return;
        }
        
        var shipPosition = NetworkSyncer.Get().shipPosition.Value;
        var vectorToShipWithoutHeight = new Vector3(shipPosition.x - transform.position.x, 0, shipPosition.z - transform.position.z);
        var distanceToShipWithoutHeight = vectorToShipWithoutHeight.magnitude;

        if (distanceToShipWithoutHeight > maxDistanceToShip)
        {
            var vectorToShipWithoutHeightNormalized = new Vector3(vectorToShipWithoutHeight.x / distanceToShipWithoutHeight, 
                0,
                vectorToShipWithoutHeight.z / distanceToShipWithoutHeight);

            transform.position += vectorToShipWithoutHeightNormalized * (drawToShipSpeed * Time.deltaTime);
        }
    }

    private float GetSwayFactor()
    {
        var ret = Mathf.Sin(Time.time * swayScale);
        ret *= swayIntensity;
        return ret;
    }

    private void UpdateHeight()
    {
        if (NetworkSyncer.Get())
        {
            targetHeight = NetworkSyncer.Get().diverTargetHeight.Value;
        }

        var heightDiff = targetHeight - transform.position.y;
        var changeInHeight = Mathf.Sign(heightDiff) * Time.deltaTime * diveSpeed;
        if (Mathf.Abs(changeInHeight) > Mathf.Abs(heightDiff))
        {
            changeInHeight = heightDiff;
        }

        var pos = transform.position;
        pos.y += changeInHeight;

        pos.y = Mathf.Max(pos.y, Terrain.activeTerrain.SampleHeight(pos));
        
        transform.position = pos;
    }
}