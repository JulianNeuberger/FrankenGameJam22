using UnityEngine;
using UnityEngine.UIElements;

public class BoatMovementController : MonoBehaviour
{
    public float forwardSpeed = 15f;
    public float turnSpeedDegrees = 10f;
    public float diveSpeed = 1f;

    public float targetHeight = 0f;

    public float swayIntensity = .1f;
    public float swayScale = 5f;

    
    private void Start()
    {
        targetHeight = transform.position.y;
    }
    
    private void Update()
    {
        UpdateLateralMovement();
        UpdateHeight();
        UpdateRotation();

        if (NetworkSyncer.Get())
        {
            //NetworkSyncer.Get().diverPosition.Value = transform.position;
        }
    }

    private void UpdateRotation()
    {
        var rotation = new Vector3(0, turnSpeedDegrees * Time.deltaTime * Input.GetAxis("Horizontal"), 0);
        rotation.y += GetSwayFactor();
        transform.Rotate(rotation);
    }
    
    private void UpdateLateralMovement()
    {
        transform.position += transform.forward * (forwardSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
        transform.position -= transform.right * GetSwayFactor();
    }

    private float GetSwayFactor()
    {
        var ret = Mathf.PerlinNoise(Time.deltaTime * swayScale, 0);
        ret -= .5f;
        ret *= 2f;
        ret *= swayIntensity;
        return ret;
    }

    private void UpdateHeight()
    {
        var heightDiff = targetHeight - transform.position.y;
        var changeInHeight = Mathf.Sign(heightDiff) * Time.deltaTime * diveSpeed;
        if (Mathf.Abs(changeInHeight) > Mathf.Abs(heightDiff))
        {
            changeInHeight = heightDiff;
        }

        var pos = transform.position;
        pos.y += changeInHeight;
        transform.position = pos;
    }
}