using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ButtonHostType : MonoBehaviour
{
    [Tooltip("Add a text when to this feild when it is either Create Room< or Join Room ")]
    
    public Text RoomName;

    public enum ButtonType
    {
        LoginToServer,
        CreateRoomInfo,
        JoinRandomRoom,
        ShowRoomList,
        CreateRoom,
        JoinRoom

    }

   
    private NetworkManager _networkManager;
    public ButtonType buttonType = ButtonType.LoginToServer;

    // Start is called before the first frame update
    void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
        Button button = GetComponent<Button>();
        AddFunctionEventToButton(button);
    }


    public void AddFunctionEventToButton(Button button)
    {
        if(buttonType==ButtonType.LoginToServer)
        {
            button.onClick.AddListener(() =>
                {
                _networkManager.LoginToServer();
            });
            
        }

        if (buttonType == ButtonType.ShowRoomList)
        {
            button.onClick.AddListener(() =>
            {
                _networkManager.ShowRoomList();
            });

        }

        if (buttonType == ButtonType.CreateRoomInfo)
        {
            button.onClick.AddListener(() =>
            {
                _networkManager.OnCreateRoomBtnClicked();
            });

        }

        if (buttonType == ButtonType.JoinRandomRoom)
        {
            button.onClick.AddListener(() =>
            {
                _networkManager.JoinRandomRoom();
            });

        }

        if(buttonType==ButtonType.CreateRoom)
        {
            button.onClick.AddListener(() =>
            {
                _networkManager.CreateRoom(RoomName.text);
            });
        }

        if (buttonType == ButtonType.JoinRoom)
        {
            button.onClick.AddListener(() =>
            {
                _networkManager.JoinToARoom(RoomName.text);
            });
        }
    }

}
