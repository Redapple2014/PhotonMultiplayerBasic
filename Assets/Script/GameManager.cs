using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        LoadPlayer();
    }


    private void LoadPlayer()
    {
        Debug.Log("StartGame!");

        // on rejoin, we have to figure out if the spaceship exists or not
        // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate


        float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
        float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
        float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
        Vector3 position = new Vector3(x, 0.0f, z);
        Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

        PhotonNetwork.Instantiate("Spaceship", position, rotation, 0);
    }

    #region PUN callbacks

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconneted reason: " + cause);
        SceneManager.LoadScene("MainMenu");
    }


    public override void OnLeftRoom()
    {
        //Since we are disconnecting the user from the server as he can't connect him again to same server
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Check if there is any more player left in the game and do accoriding");
        if(PhotonNetwork.CurrentRoom.PlayerCount==0)
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    #endregion PUN callbacks
}
