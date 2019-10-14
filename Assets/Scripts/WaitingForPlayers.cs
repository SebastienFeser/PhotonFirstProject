using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class WaitingForPlayers : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI waitingForPlayerText;
    [SerializeField] int numberOfPlayersToJoin;

    private void Update()
    {
        if (numberOfPlayersToJoin != PhotonNetwork.PlayerList.Length)
        {
            waitingForPlayerText.text = "Waiting for " + (numberOfPlayersToJoin - PhotonNetwork.PlayerList.Length) + " players";
        }
        else
        {
            waitingForPlayerText.text = "All players are here";
        }
    }
}
