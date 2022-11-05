using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button diverBtn;
    [SerializeField] private Button captainBtn;
    [SerializeField] private TMP_InputField ipInput;

    private bool iAmTheCaptainNow = false;

    [SerializeField] private NetworkObject syncManager;
    [SerializeField] private GameObject mainMenuCanvas;

    [SerializeField] private GameObject diverLooseScreen;
    [SerializeField] private GameObject captainLooseScreen;
    [SerializeField] private GameObject diverWinScreen;
    [SerializeField] private GameObject captainWinScreen;


    private void Awake()
    {
        diverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();

            SceneManager.LoadScene("DiverScene", LoadSceneMode.Additive);

            var instance = Instantiate(syncManager);
            instance.Spawn();

            mainMenuCanvas.SetActive(false);
            iAmTheCaptainNow = false;
        });


        captainBtn.onClick.AddListener(() =>
        {
            var ipAddress = ipInput.text;
            Debug.Log($"IP Address entered: {ipAddress}");

            if(ipAddress == "")
            {
                ipAddress = "127.0.0.1";
                Debug.Log($"No IP Address entered, using default {ipAddress}");
            }

            //TODO: Validate IP address?
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                ipAddress,
                (ushort)7777 // TODO: specify port
            );

            NetworkManager.Singleton.StartClient();

            SceneManager.LoadScene("CaptainScene", LoadSceneMode.Additive);

            mainMenuCanvas.SetActive(false);
            iAmTheCaptainNow = true;
        });
    }



    public void LooseGame()
    {
        if(iAmTheCaptainNow)
        {
            captainLooseScreen.SetActive(true);
        }
        else
        {
            diverLooseScreen.SetActive(true);
        }
    }


    public void WinGame()
    {
        if (iAmTheCaptainNow)
        {
            captainWinScreen.SetActive(true);
        }
        else
        {
            diverWinScreen.SetActive(true);
        }
    }
}
   