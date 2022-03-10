using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    public GameObject loadingScreen;
    public GameObject menuButtons;
    public GameObject createRoomScreen;
    public GameObject exitButton;
    public GameObject roomScreen;
    public GameObject errorScreen;
    public GameObject roomBrowserScreen;
    public RoomButton roomButton;
    private List<RoomButton> allRoomButtons = new List<RoomButton>();
    public TMP_Text errorText;
    public TMP_Text roomNameText;
    public TMP_InputField createRoomInput;
    public TMP_Text loadingText;
    public void Awake()
    {
        instance = this;
    }
    public void CloseMenus()
    {
        menuButtons.SetActive(false);
        loadingScreen.SetActive(false);
        createRoomScreen.SetActive(false);
        roomScreen.SetActive(false);
        errorScreen.SetActive(false);
        roomBrowserScreen.SetActive(false);
    }
    public void Start()
    {
        CloseMenus();
        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to Network";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        loadingText.text = "Joining Lobby";

    }
    public override void OnJoinedLobby()
    {
        CloseMenus();
        menuButtons.SetActive(true);
    }
    public void OpenCreateRoomPanel()
    {
        CloseMenus();
        createRoomScreen.SetActive(true);
        exitButton.SetActive(true);
    }

    public void OpenMainMenu()
    {
        CloseMenus();
        exitButton.SetActive(false);
        menuButtons.SetActive(true);
    }
    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(createRoomInput.text))
        {
            RoomOptions options = new RoomOptions();
            //vai ser changable.
            options.MaxPlayers = 8;
            PhotonNetwork.CreateRoom(createRoomInput.text,options);
            CloseMenus();
            loadingText.text = "Creating room";
            loadingScreen.SetActive(true);
        }
    }
    public override void OnJoinedRoom()
    {
        CloseMenus();
        exitButton.SetActive(false);
        roomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseMenus();
        exitButton.SetActive(true);
        errorScreen.SetActive(true);
        errorText.text = "["+returnCode+"]Failed to create room: " + message;

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CloseMenus();
        exitButton.SetActive(true);
        errorScreen.SetActive(true);
        errorText.text = "[" + returnCode + "]Failed to join room: " + message;
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenus();
        loadingText.text = "Leaving room";
        loadingScreen.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        CloseMenus();
        OpenMainMenu();
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void OpenRoomBrowser()
    {
        CloseMenus();
        roomBrowserScreen.SetActive(true);
        exitButton.SetActive(true);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomButton rb in allRoomButtons){
            Destroy(rb.gameObject);
        }
        allRoomButtons.Clear();
        roomButton.gameObject.SetActive(false);
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount!=roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomButton newButton = Instantiate(roomButton, roomButton.transform.parent);
                newButton.SetButtonDetails(roomList[i]);
                newButton.gameObject.SetActive(true);

                allRoomButtons.Add(newButton);
            }
        }
    }
}