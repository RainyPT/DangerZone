using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

public class Launcher : MonoBehaviourPunCallbacks
{
    private string publicIP = "localhost";
    public static Launcher instance;
    public GameObject loadingScreen;
    public GameObject errorBox;
    public TMP_Text errorBoxText;
    public GameObject menuButtons;
    public GameObject createRoomScreen;
    public GameObject exitButton;
    public GameObject roomScreen;
    public GameObject roomBrowserScreen;
    public GameObject credsScreen;
    public GameObject startGameButton;
    public RoomButton roomButton;
    private List<RoomButton> allRoomButtons = new List<RoomButton>();
    private List<TMP_Text> playerLabels = new List<TMP_Text>();
    public TMP_Text roomNameText,playerNameLabel;
    public TMP_InputField createRoomInput;
    public TMP_Text loadingText;
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    private string[] levelsToPlay = {"Mapa1"};
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
        roomBrowserScreen.SetActive(false);
        credsScreen.SetActive(false);
    }
    public void Start()
    {
        CloseMenus();
        credsScreen.SetActive(true);
    }
    public override void OnConnectedToMaster()
    {
        CloseMenus();
        showLoadingScreen("Joining Lobby");
        PhotonNetwork.JoinLobby();

    }
    private void ConnectToMasterServer(string username)
    {
        showLoadingScreen("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        PlayerPrefs.SetString("username", username);
        PhotonNetwork.NickName = username;
    }
    private void showErrorBox(string error)
    {
        errorBox.SetActive(true);
        errorBoxText.text = error;
    }
    public void CallRegister()
    {
        StartCoroutine(Register());
    }
    IEnumerator Register()
    {
        if (usernameField.text.Length != 0 || passwordField.text.Length!=0)
        {
            showLoadingScreen("Registering");
            WWWForm form = new WWWForm();
            form.AddField("username", usernameField.text);
            form.AddField("password", passwordField.text);
            UnityWebRequest www = UnityWebRequest.Post("http://"+ publicIP + ":4000/register", form);
            yield return www.SendWebRequest();

            if (www.responseCode != 200)
            {
                loadingScreen.SetActive(false);
                showErrorBox(www.downloadHandler.text);
            }
            else
            {
                CloseMenus();
                ConnectToMasterServer(usernameField.text);
            }
        }
        else
        {
            showErrorBox("Please fill the input boxes!");
        }
    }

    public void CallLogin()
    {
      StartCoroutine(Login());
    }
    IEnumerator Login()
    {
        if (usernameField.text.Length != 0 || passwordField.text.Length != 0)
        {
            showLoadingScreen("Authenticating");
            WWWForm form = new WWWForm();
            form.AddField("username", usernameField.text);
            form.AddField("password", passwordField.text);
            UnityWebRequest www = UnityWebRequest.Post("http://"+ publicIP + ":4000/login", form);
            yield return www.SendWebRequest();

            if (www.responseCode != 200)
            {
                loadingScreen.SetActive(false);
                showErrorBox(www.downloadHandler.text);
            }
            else
            {
                CloseMenus();
                ConnectToMasterServer(usernameField.text);
            }
        }
        else
        {
            showErrorBox("Please fill the input boxes!");
        }
    }

    public override void OnJoinedLobby()
    {
        OpenMainMenu();
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
            showLoadingScreen("Creating Room");
        }
    }
    public override void OnJoinedRoom()
    {
        CloseMenus();
        exitButton.SetActive(false);
        roomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        startGameButton.SetActive(false);
        ListAllPlayers();
    }
    private void ListAllPlayers()
    {
        foreach(TMP_Text playerLabel in playerLabels)
        {
            Destroy(playerLabel.gameObject);
        }
        playerLabels.Clear();
        Player[] playersInRoom = PhotonNetwork.PlayerList;
        for(int i = 0; i < playersInRoom.Length; i++)
        {
            TMP_Text t = Instantiate(playerNameLabel,playerNameLabel.transform.parent);
            t.text = playersInRoom[i].NickName;
            t.gameObject.SetActive(true);
            playerLabels.Add(t);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TMP_Text t = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        t.text = newPlayer.NickName;
        t.gameObject.SetActive(true);
        playerLabels.Add(t);

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ListAllPlayers();
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        showErrorBox("[" + returnCode + "]Failed to create room: " + message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        showErrorBox( "[" + returnCode + "]Failed to join room: " + message);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenus();
        showLoadingScreen("Leaving Room");
    }
    public override void OnLeftRoom()
    {
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
    public void showLoadingScreen(string loadingMsg)
    {
        loadingText.text = loadingMsg;
        loadingScreen.SetActive(true);
    }
    public void JoinRoom(RoomInfo rinfo)
    {
        PhotonNetwork.JoinRoom(rinfo.Name);
        CloseMenus();
        showLoadingScreen("Joining Room");
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(levelsToPlay[0]);
    }
}