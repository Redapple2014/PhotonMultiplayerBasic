using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetails : MonoBehaviour
{
    public Text PlayerName;
    private int ownerId;
    public Player Player;

       public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerName.text = playerName;
    }

   

    
}


