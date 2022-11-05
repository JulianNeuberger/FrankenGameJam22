using UnityEngine;

public class Compass : MonoBehaviour
{
    public float jiggleScale = 5.0f;
        
    void Update()
    {
        var rot = transform.rotation.eulerAngles;
        var targetRot = Mathf.PerlinNoise(0, Time.time) * jiggleScale;
        rot.y = Mathf.LerpAngle(rot.y, targetRot, Time.deltaTime);
        transform.rotation = Quaternion.Euler(rot);
    }
}
