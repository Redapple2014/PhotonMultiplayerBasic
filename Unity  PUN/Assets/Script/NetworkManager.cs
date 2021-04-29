using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public bool IsSyncScene;
    public bool CloseRoomAfterGameStart;

    [Header("Login Panel")]
    public GameObject LoginPanel;

    public InputField PlayerNameInput;


    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    public InputField RoomNameInputField;
    public InputField MaxPlayersInputField;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;
    public GameObject _roomContentParent;

    [SerializeField]
    private RoomListings _roomListings;
    private List<RoomListings> _listing = new List<RoomListings>();

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;
    public GameObject PlayerListEntryPrefab;
    public GameObject _contentParent;

    private Dictionary<int, GameObject> playerListEntries;
    private List<PlayerDetails> playerDetails = new List<PlayerDetails>();

    [Header("Panels")]
    public List<GameObject> m_Panels;

    #region Unity Functions
    private void Start()
    {
        PlayerNameInput.text = "Player " + Random.Range(100, 500);

       
    }

    #endregion Unity Functions

    #region Custom Function

    //if you want to connect to master when the game start the call this function on start
    //Contects to master Server
    public void LoginToServer()
    {
        Debug.Log("Entered");
        string playerName = PlayerNameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.AutomaticallySyncScene = IsSyncScene;
            PhotonNetwork.LocalPlayer.NickName = playerName;
            if (!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }

    /// <summary>
    /// Allows you to create room on the server
    /// callbacks-OncreatedRoom / OnjoinedRoom/OnCreateRoomFailed
    /// </summary>
    public void CreateRoom(string roomName=null)
    {
        if(roomName==null)
        {
            roomName = "RoomName" + Random.Range(1000, 9999);
        }
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = byte.Parse(this.MaxPlayersInputField.text)
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);

    }

    public void OnCreateRoomBtnClicked()
    {
        ToggleScreen("CreateRoomPanel");
    }

    /// <summary>
    /// Allows you to join to room on the server
    /// callbacks- OnjoinedRoom/OnjoinedRoomFailed
    /// </summary>
    /// <param name="roomName"></param>
    public void JoinToARoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ToggleScreen("SelectionPanel");
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
        ToggleScreen("SelectionPanel");

    }

    /// <summary>
    /// Allows you to join Random room on the server
    /// if no room available then it will create a new random  room
    /// callbacks-OnjoinedRoom / OnjoinedRoomFail/OnCreateRoomFailed
    /// </summary>
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void ShowRoomList()
    {
        ToggleScreen("RoomListPanel");
    }

    public void OnRoomBack()
    {
        ToggleScreen("SelectionPanel");
    }

    public void OnStartGameButtonClicked()
    {
        if (CloseRoomAfterGameStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }

        PhotonNetwork.LoadLevel("Game Scene");
    }

    public void ToggleScreen(string _screenName)
    {
        foreach (GameObject src in m_Panels)
        {
            src.SetActive(_screenName.Equals(src.name));
        }
    }

    #endregion Custom Function


    #region PUN CallBacks 

    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully Coneected To Master Server");
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

        ToggleScreen("SelectionPanel");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnCreatedRoom()
    {
        //   base.OnCreatedRoom();
        Debug.Log("Welcome to the room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation Failed" + returnCode + " ,,," + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Failed to join random room " + returnCode + "    " + message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed to join room " + returnCode + "    " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("You have joined the room" + PhotonNetwork.CurrentRoom.PlayerCount);

        //   cachedRoomList.Clear();
        ToggleScreen("PlayerListPanel");

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            //int index = playerDetails.FindIndex(x => x.Player.ActorNumber == p.ActorNumber);
            //if (index != -1)
            //{
            //    Destroy(playerDetails[index].gameObject);
            //    playerDetails.RemoveAt(index);
            //}
            GameObject entry = Instantiate(PlayerListEntryPrefab, _contentParent.transform);
            PlayerDetails _temp = entry.GetComponent<PlayerDetails>();
            _temp.Initialize(p.ActorNumber, p.NickName);
            playerDetails.Add(_temp);
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            //int index = playerDetails.FindIndex(x => x.Player.ActorNumber == p.ActorNumber);
            //if (index != -1)
            //{
            //    Destroy(playerDetails[index].gameObject);
            //    playerDetails.RemoveAt(index);
            //}
            GameObject entry = Instantiate(PlayerListEntryPrefab, _contentParent.transform);
            PlayerDetails _temp = entry.GetComponent<PlayerDetails>();
            _temp.Initialize(p.ActorNumber, p.NickName);
            playerDetails.Add(_temp);

        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerDetails.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(playerDetails[index].gameObject);
            playerDetails.RemoveAt(index);
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentLobby.Name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        Debug.Log("Room list" + roomList.Count);

        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _listing.FindIndex(x => x._roomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_listing[index].gameObject);
                    _listing.RemoveAt(index);
                }
            }
            else
            {
                RoomListings listing = Instantiate(_roomListings, _roomContentParent.transform);
                if (listing != null)
                {
                    listing.Initialize(info);
                    _listing.Add(listing);
                }
            }

        }
    }

    public override void OnLeftRoom()
    {
       // ToggleScreen("SelectionPanel");
        Debug.Log("You have left the room");
        foreach (PlayerDetails entry in playerDetails)
        {
            if(entry!=null)
            Destroy(entry.gameObject);
        }

    }
    #endregion PUN CallBack
       

}