using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkingLoader : MonoBehaviour
{

    private void Awake()
    {
        SceneManager.LoadScene("NetworkingScene", LoadSceneMode.Additive);
    }

}
