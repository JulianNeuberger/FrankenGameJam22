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


    private void Update()
    {
        if (NetworkSyncer.Get())
        {
            if (NetworkSyncer.Get().gameLost.Value)
            {
                ShowGameLost();
            }
            else if (NetworkSyncer.Get().gameWon.Value)
            {
                ShowGameWon();
            }
        }
    }



    public void ShowGameLost()
    {
        if(iAmTheCaptainNow)
        {
            captainLooseScreen.SetActive(true);
            var looseScreenCanvasGroup = captainLooseScreen.GetComponent<CanvasGroup>();
            if(looseScreenCanvasGroup.alpha < 0.6)
            {
                looseScreenCanvasGroup.alpha += 0.2f * Time.deltaTime;
            }
        }
        else
        {
            diverLooseScreen.SetActive(true);
            var looseScreenCanvasGroup = diverLooseScreen.GetComponent<CanvasGroup>();
            looseScreenCanvasGroup.alpha += 0.3f * Time.deltaTime;
        }
    }


    public void ShowGameWon()
    {
        if (iAmTheCaptainNow)
        {
            captainWinScreen.SetActive(true);
            var winScreenCanvasGroup = captainWinScreen.GetComponent<CanvasGroup>();
            if (winScreenCanvasGroup.alpha < 0.6)
            {
                winScreenCanvasGroup.alpha += 0.2f * Time.deltaTime;
            }
        }
        else
        {
            diverWinScreen.SetActive(true);
            var winScreenCanvasGroup = diverWinScreen.GetComponent<CanvasGroup>();
            winScreenCanvasGroup.alpha += 0.3f * Time.deltaTime;
        }
    }
}
   