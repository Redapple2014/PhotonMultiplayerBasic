using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Photon.Pun;


//[CustomEditor(typeof(NetworkManager))]
public class PhotonEditorScript : Editor
{
    #region Custom Functions


    [MenuItem("PhotonUnityBasicButtons/Login/Create LoginTo Server Button", false, 0)]
    public static void CreateLoginToServerButton()//(string btn_name, string componentName)
    {
        Debug.Log("Entere ");
        CreateCommonButton("Login To Server", ButtonHostType.ButtonType.LoginToServer);
    }

    [MenuItem("PhotonUnityBasicButtons/Host/Create Room info button", false, 1)]
    public static void CreateHostRoomBtn()
    {
        CreateCommonButton("Enter Room info", ButtonHostType.ButtonType.CreateRoomInfo);
    }

    [MenuItem("PhotonUnityBasicButtons/Host/Create Join Random Room button", false, 2)]
    public static void CreateRandomRoomBtn()//(string btn_name, string componentName)
    {
        CreateCommonButton("Join Random Room", ButtonHostType.ButtonType.JoinRandomRoom);
    }

    [MenuItem("PhotonUnityBasicButtons/Host/Create Show Room List button", false, 4)]
    public static void CreateShowRoomBtn()//(string btn_name, string componentName)
    {
        CreateCommonButton("Show Room List", ButtonHostType.ButtonType.ShowRoomList);
    }

    [MenuItem("PhotonUnityBasicButtons/Host/Create Show Player List button", false, 5)]
    public static void ShowPlayerList()//(string btn_name, string componentName)
    {
        CreateCommonButton("Create Room", ButtonHostType.ButtonType.CreateRoom);
    }

    [MenuItem("PhotonUnityBasicButtons/Host/Create Join Room button", false, 3)]
    public static void CreateJoinRoomBtn()//(string btn_name, string componentName)
    {
        CreateCommonButton("Join Room", ButtonHostType.ButtonType.JoinRoom);
    }

    public static void CreateCommonButton(string btnName, ButtonHostType.ButtonType _type)
    {

        if (Selection.activeGameObject == null)
        {
            EditorApplication.ExecuteMenuItem("GameObject/UI/Button");
            GameObject BtnObj = Selection.activeGameObject;
            BtnObj.name = btnName;
            Text _child = BtnObj.transform.GetChild(0).GetComponent<Text>();
            _child.text = btnName;
            BtnObj.AddComponent<ButtonHostType>();
            BtnObj.GetComponent<ButtonHostType>().buttonType = _type;
        }
        else
        {
            GameObject btnObj = Selection.activeGameObject;
            btnObj.AddComponent<Button>();
            btnObj.AddComponent<ButtonHostType>();
            btnObj.GetComponent<ButtonHostType>().buttonType = _type;
            btnObj.name = btnName;
        }
    }

    #endregion Custom Functions
}



