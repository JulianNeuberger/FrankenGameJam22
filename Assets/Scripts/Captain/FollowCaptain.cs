using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCaptain : MonoBehaviour
{

    public GameObject captain;

    void LateUpdate()
    {
        transform.position = captain.transform.position + new Vector3(0, 0, -10);
    }
}
