using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDispaching : MonoBehaviourPunCallbacks
{
    struct SpawnPoint
    {
        Vector3 spawnPoint;
        bool isUsed;
    }




    private void Start()
    {
        PhotonNetwork.Instantiate("Ball", new Vector3(0, 0, 0), Quaternion.identity, 0);
        //PhotonNetwork.Instantiate()
    }
}
