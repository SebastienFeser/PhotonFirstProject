using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player ", newPlayer.NickName, " entered the room");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("IsMasterClient ", PhotonNetwork.IsMasterClient);

            LoadGameScene();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player ", otherPlayer.NickName, " left the room");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("IsMasterClient ", PhotonNetwork.IsMasterClient);

            LoadGameScene();
        }
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods

    void LoadGameScene()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Trying to load level but we are not the master Client");
        }

        PhotonNetwork.LoadLevel("GameScene");

    }
    #endregion
}
