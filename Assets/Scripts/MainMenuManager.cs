using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button diverBtn;
    [SerializeField] private Button captainBtn;
    [SerializeField] private TMP_InputField ipInput;


    private void Awake()
    {
        diverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });


        captainBtn.onClick.AddListener(() =>
        {
            var ipAddress = ipInput.text;
            Debug.Log($"IP Address entered: {ipAddress}");

            //TODO: Validate IP address?
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                ipAddress,
                (ushort)7777 // TODO: specify port
            );

            NetworkManager.Singleton.StartClient();
        });
    }

}
   