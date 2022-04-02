using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;
    public GameObject playerPrefab;
    public Transform spawnPrefabPoint;
    private GameObject player;
    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            Transform spawnPoint = spawnPrefabPoint;
            player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
            player.transform.Find("Player Camera").gameObject.SetActive(true);
            player.transform.Find("BodyParts").Find("Goggles").GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
