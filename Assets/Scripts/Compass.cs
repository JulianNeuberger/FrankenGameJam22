using System;
using UnityEngine;

public class Compass : MonoBehaviour
{
    private void Start()
    {
        var rot = transform.rotation.eulerAngles;
        rot.y = 0;
        transform.rotation = Quaternion.Euler(rot);
    }

    void Update()
    {
        var rot = transform.rotation.eulerAngles;
        rot.y = Mathf.LerpAngle(rot.y, 0, Time.deltaTime);
        transform.rotation = Quaternion.Euler(rot);
    }
}
