using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Launcher : MonoBehaviour
{
    string gameVersion = "1";
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        Connect();
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
}
