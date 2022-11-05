using UnityEngine;

public class Compass : MonoBehaviour
{
    void Update()
    {
        var rot = transform.rotation.eulerAngles;
        rot.y = Mathf.LerpAngle(rot.y, 0, Time.deltaTime);
        transform.rotation = Quaternion.Euler(rot);
    }
}
