using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI player1Text;
    [SerializeField] TextMeshProUGUI player2Text;
    [SerializeField] TextMeshProUGUI player3Text;
    [SerializeField] TextMeshProUGUI player4Text;

    [SerializeField] GameObject startButton;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        player1Text.text = "No player 1";
        player2Text.text = "No player 2";
        player3Text.text = "No player 3";
        player4Text.text = "No player 4";

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }




        if (PhotonNetwork.PlayerList.Length >= 1)
        {
            player1Text.text = PhotonNetwork.PlayerList[0].NickName;
        }
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            player2Text.text = PhotonNetwork.PlayerList[1].NickName;
        }
        if (PhotonNetwork.PlayerList.Length >= 3)
        {
            player3Text.text = PhotonNetwork.PlayerList[2].NickName;
        }
        if (PhotonNetwork.PlayerList.Length == 4)
        {
            player4Text.text = PhotonNetwork.PlayerList[3].NickName;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }



    [PunRPC]
    void LoadLevel()
    {

        PhotonNetwork.LoadLevel("GameScene");
    }

    public void StartButton()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        //PhotonNetwork.LoadLevel("GameScene");
        photonView.RPC("LoadLevel", RpcTarget.All);
    }

    public void QuitButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("FinalLobby");
    }


}
