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
    public static Launcher instance;
    public GameObject loadingScreen;
    public GameObject menuButtons;
    public GameObject createRoomScreen;
    public GameObject exitButton;
    public GameObject roomScreen;
    public GameObject errorScreen;
    public GameObject roomBrowserScreen;
    public GameObject loginScreen;
    public GameObject registerScreen;
    public GameObject startGameButton;
    public RoomButton roomButton;
    private List<RoomButton> allRoomButtons = new List<RoomButton>();
    private List<TMP_Text> playerLabels = new List<TMP_Text>();
    public TMP_Text errorText;
    public TMP_Text roomNameText,playerNameLabel;
    public TMP_InputField createRoomInput;
    public TMP_Text loadingText;


    public TMP_InputField usernameRegisterField;
    public TMP_InputField passwordRegisterField;

    public TMP_InputField usernameLoginField;
    public TMP_InputField passwordLoginField;
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
        errorScreen.SetActive(false);
        roomBrowserScreen.SetActive(false);
        loginScreen.SetActive(false);
        registerScreen.SetActive(false);
    }
    public void Start()
    {
        CloseMenus();
        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to Network";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnConnectedToMaster()
    {
        CloseMenus();
        loginScreen.SetActive(true);
        
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }
    IEnumerator Register()
    {
        loadingScreen.SetActive(true);
        loadingText.text = "Registering";
        WWWForm form = new WWWForm();
        form.AddField("username", usernameRegisterField.text);
        form.AddField("password", passwordRegisterField.text);
        UnityWebRequest www = UnityWebRequest.Post("http://161.230.225.30:4000/register", form);
        yield return www.SendWebRequest();

        if (www.responseCode != 200)
        {
            CloseMenus();
            showErrorScreen(www.downloadHandler.text);
        }
        else
        {
            PlayerPrefs.SetString("username", usernameLoginField.text);
            PhotonNetwork.JoinLobby();
            PhotonNetwork.NickName = usernameLoginField.text;
            OpenMainMenu();
        }
    }

    public void CallLogin()
    {
      StartCoroutine(Login());
    }
    IEnumerator Login()
    {
        loadingScreen.SetActive(true);
        loadingText.text = "Authenticating";
        WWWForm form = new WWWForm();
        form.AddField("username", usernameLoginField.text);
        form.AddField("password", passwordLoginField.text);
        UnityWebRequest www = UnityWebRequest.Post("http://161.230.225.30:4000/login", form);
        yield return www.SendWebRequest();

        if (www.responseCode !=200)
        {
            CloseMenus();
            showErrorScreen(www.downloadHandler.text);
        }
        else
        {
            PlayerPrefs.SetString("username", usernameLoginField.text);
            PhotonNetwork.JoinLobby();
            PhotonNetwork.NickName = usernameLoginField.text;
            OpenMainMenu();
        }
    }

    private void showErrorScreen(string erro)
    {
        errorText.text = erro;
        errorScreen.SetActive(true);
        exitButton.SetActive(true);
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
    public void OpenRegisterMenu()
    {
        CloseMenus();
        registerScreen.SetActive(true);
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
    public void JoinRoom(RoomInfo rinfo)
    {
        PhotonNetwork.JoinRoom(rinfo.Name);
        CloseMenus();
        loadingText.text = "Joining Room";
        loadingScreen.SetActive(true);
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(levelsToPlay[0]);
    }
}