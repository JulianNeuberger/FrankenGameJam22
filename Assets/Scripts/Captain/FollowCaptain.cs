using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCaptain : MonoBehaviour
{

    public GameObject captain;
    public float zValue = -10f;

    void LateUpdate()
    {
        transform.position = captain.transform.position + new Vector3(0, 0, zValue);
    }
}
