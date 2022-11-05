using UnityEngine;

public class BoatMovementController : MonoBehaviour
{
    public float forwardSpeed = 15f;
    public float turnSpeedDegrees = 10f;

    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        //_rigidbody.AddForce(transform.forward * (forwardSpeed * Time.deltaTime * Input.GetAxis("Vertical")));
        
        transform.position += transform.forward * (forwardSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
        transform.Rotate(new Vector3(0, turnSpeedDegrees * Time.deltaTime * Input.GetAxis("Horizontal"), 0));

        if (NetworkSyncer.Get())
        {
            NetworkSyncer.Get().diverPosition.Value = transform.position;
        }
    }
}