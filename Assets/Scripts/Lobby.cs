using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject loginPannel;
    [SerializeField] GameObject selectionPannel;
    [SerializeField] GameObject joiningRoomPannel;

    [SerializeField] TMP_InputField playerNameField;
    
    private void Start()
    {
        loginPannel.SetActive(true);
        selectionPannel.SetActive(false);
    }
    
    public void LoginButton()
    {
        string playerName = playerNameField.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = "Player " + Random.Range(1000, 10000);
        }

        PhotonNetwork.ConnectUsingSettings();

        loginPannel.SetActive(false);
    
    }

    public void JoinRandomRoomButton()
    {
        joiningRoomPannel.SetActive(true);
        selectionPannel.SetActive(false);

        PhotonNetwork.JoinRandomRoom();
    }

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        selectionPannel.SetActive(true);
        loginPannel.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1000, 10000);

        Photon.Realtime.RoomOptions options = new Photon.Realtime.RoomOptions { MaxPlayers = 4 };

        PhotonNetwork.CreateRoom(roomName, options, null);
        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        selectionPannel.SetActive(true);
        joiningRoomPannel.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("WaitingRoom");
    }

    #endregion

}
