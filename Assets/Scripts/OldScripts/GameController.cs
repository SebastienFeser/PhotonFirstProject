using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks
{
    enum GameState
    {
        WAITING_FOR_PLAYERS,
        READY,
        GAME,
        END_GAME
    }
    int playersInRoom;
    int playersLeftInGame;
    [SerializeField] List<Vector3> playerSpawn; 

    private void Update()
    {
        
    }

    void PlayerEnterRoom()
    {

    }

    //Used to Instantiate players entering the room
    private void OnConnectedToServer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playersInRoom = 0;
        }

        PhotonNetwork.Instantiate("Ball", playerSpawn[playersInRoom], Quaternion.identity, 0);
        playersInRoom += 1;
    }

}
