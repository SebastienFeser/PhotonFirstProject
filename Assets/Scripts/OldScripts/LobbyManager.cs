using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject LoginPannel;
    [SerializeField] InputField playerNameInputField;

    [SerializeField] GameObject SelectionPannel;
    [SerializeField] GameObject CreateRoomPannel;
    [SerializeField] InputField roomNameInputField;
    [SerializeField] GameObject JoinRandomRoomPannel;
    [SerializeField] GameObject RoomListPannel;

    [SerializeField] GameObject InsideRoomPannel;

    [SerializeField] byte maxPlayers = 4;

    [SerializeField] GameObject PlayerListPrefab;

    //[SerializeField] Button StartButton;

    private Dictionary<string, RoomInfo> roomListInfo;
    private Dictionary<string, GameObject> roomList;
    private Dictionary<int, GameObject> playerList;

    #region Pun Callbacks

    public override void OnConnectedToMaster()
    {
        //TO CHECK
        ActivatePannel(SelectionPannel);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //TODO
    }

    public override void OnLeftLobby()
    {
        //TODO
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ActivatePannel(SelectionPannel);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ActivatePannel(SelectionPannel);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        ActivatePannel(InsideRoomPannel);

        //TODO

        if (playerList == null)
        {
            playerList = new Dictionary<int, GameObject>();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerInRoom = Instantiate(PlayerListPrefab);
            playerInRoom.transform.SetParent(InsideRoomPannel.transform);
            playerInRoom.transform.localScale = Vector3.one;
            //playerInRoom.GetComponent<PlayerListEntry>

        //TODO
        }
    }

    public override void OnLeftRoom()
    {
        ActivatePannel(SelectionPannel);
        foreach (GameObject playerInRoom in playerList.Values)
        {
            //Rename
            Destroy(playerInRoom.gameObject);
        }

        playerList.Clear();
        playerList = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerInRoom = Instantiate(PlayerListPrefab);
        //TODO
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerList[otherPlayer.ActorNumber].gameObject);
        playerList.Remove(otherPlayer.ActorNumber);

        //StartButton.gameObject.SetActive(CheckPlayersReady());

        //TODO
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (playerList == null)
        {
            playerList = new Dictionary<int, GameObject>();
        }

        GameObject playerInRoom;
        if (playerList.TryGetValue(target.ActorNumber, out playerInRoom))
        {
            object playerReady;
            //if (changedProps.TryGetValue)
            //TODO
        }

        //TODO

        //StartGameButton.gameObject.SetActive
    }



    #endregion

    #region Button Click Functions

    public void BackButton()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        ActivatePannel(SelectionPannel);
    }

    public void CreateRoomButton() //TODO
    {
        string roomName = roomNameInputField.text;
        if (roomName == string.Empty)
        {
            //Choosing a name for the room if empty
            roomName = "Room " + Random.Range(1000, 10000);
        }
        byte maxPlayers;
    }

    public void OnJoinRandomRoomButton()
    {
        ActivatePannel(JoinRandomRoomPannel);

        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
        //TOCHECK
        //ActivatePannel(SelectionPannel);
    }

    public void LoginButton()
    {
        string playerName = playerNameInputField.text;

        if (!playerName.Equals(""))
        {
            //Connect to the server with the player Name from the Input Field
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }

    }

    public void RoomListButton()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePannel(RoomListPannel);
    }

    public void StartGameButton()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //TODO
        //PhotonNetwork.LoadLevel("");
    }

    private void ActivatePannel(GameObject pannel)
    {
        //Desactivation of every pannels
        SelectionPannel.SetActive(false);
        CreateRoomPannel.SetActive(false);
        SelectionPannel.SetActive(false);
        JoinRandomRoomPannel.SetActive(false);
        RoomListPannel.SetActive(false);
        InsideRoomPannel.SetActive(false);

        //Activation of the pannel we need
        pannel.SetActive(true);
    }

    #endregion

    /*private bool CheckPlayersReady()
    {
        //TODO
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(TODO))
            {
                if (!(bool) isPlayerReady)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }*/

    private void ClearRoomList()
    {
        foreach (GameObject room in roomList.Values)
        {
            Destroy(room.gameObject);
        }

        roomList.Clear();
    }

    public void LocalPlayerPropertiesUpdated()
    {
        //TODO
    }


}