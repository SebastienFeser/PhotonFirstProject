using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{

    #region Private Serializable Fields
    [SerializeField] private byte maxPlayerPerRoom = 4;
    [SerializeField] GameObject identificationUI;
    [SerializeField] GameObject connectingText;
    #endregion

    #region Pivate Fields
    bool isConnecting;

    //Client's version number
    string gameVersion = "1";
    #endregion

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        //For clients to sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        identificationUI.SetActive(true);
        connectingText.SetActive(false);
    }

    #endregion

    #region Public Methods

    public void Connect()
    {
        isConnecting = true;
        identificationUI.SetActive(false);
        connectingText.SetActive(true);
        if (PhotonNetwork.IsConnected)
        {
            //To change if you want to create lobbies
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region Pun Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster was called");

        if (isConnecting)
        {
            //To change if you'd like to create lobbys
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectingText.SetActive(false);
        identificationUI.SetActive(true);
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason { 0} ", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join Random Room Failed, Creation of a new Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayerPerRoom });
        
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Client joined a room");

        PhotonNetwork.LoadLevel("GameRoom");
    }

    #endregion

}
