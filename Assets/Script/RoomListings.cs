using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomListings : MonoBehaviour
{
    public Text RoomNameText;
    public Text RoomPlayersText;
    public Button JoinRoomButton;

    private string roomName;
    public RoomInfo _roomInfo;

    private NetworkManager NetworkManager;

    public void Start()
    {
        NetworkManager = GameObject.FindObjectOfType<NetworkManager>();
        //JoinRoomButton.onClick.AddListener(() =>
        //{
        //    if (PhotonNetwork.InLobby)
        //    {
        //        PhotonNetwork.LeaveLobby();
        //    }

        //    NetworkManager.JoinToARoom(roomName);
        //});
    }

    public void Initialize(RoomInfo info )
    {
        roomName = info.Name;

        RoomNameText.text = info.Name;
        RoomPlayersText.text = (byte)info.PlayerCount + " / " + info.MaxPlayers;
    }
}

