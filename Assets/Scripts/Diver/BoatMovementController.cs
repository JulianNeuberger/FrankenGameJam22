using UnityEngine;
using UnityEngine.UIElements;

public class BoatMovementController : MonoBehaviour
{
    public float forwardSpeed = 15f;
    public float turnSpeedDegrees = 10f;
    public float diveSpeed = 1f;

    public float targetHeight = 0f;

    
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
        transform.Rotate(new Vector3(0, turnSpeedDegrees * Time.deltaTime * Input.GetAxis("Horizontal"), 0));
    }
    
    private void UpdateLateralMovement()
    {
        transform.position += transform.forward * (forwardSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
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