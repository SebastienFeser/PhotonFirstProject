﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviourPunCallbacks
{
    [SerializeField] Vector3 spawn1;    //Not Networked
    [SerializeField] Vector3 spawn2;    //Not Networked
    [SerializeField] Vector3 spawn3;    //Not Networked
    [SerializeField] Vector3 spawn4;    //Not Networked
    GameObject currentPlayerBall;       //Not Networked
    [SerializeField] TextMeshProUGUI centralText;
    string winnerName = "Nobody";                  //Networked
    bool hasIncreased = false;
    bool hasStartedCoroutine = false;


    public enum GameState
    {
        WAITING_TO_START,
        GAME,
        END_GAME
    }

    public GameState gameState;            //Networked

    bool masterClientConnected;     //Networked

    int hasLoaded;                  //Networked

    int playerAlive;                //Networked

    private void Start()
    {
        gameState = GameState.WAITING_TO_START;         //Networked
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("MasterClientConnected", RpcTarget.All);
        }

        Vector3 spawnPosition = new Vector3(0, 0, 0);
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            spawnPosition = spawn1;
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1])
        {
            spawnPosition = spawn2;
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2])
        {
            spawnPosition = spawn3;
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[3])
        {
            spawnPosition = spawn4;
        }

        currentPlayerBall = PhotonNetwork.Instantiate("Ball", spawnPosition, Quaternion.identity, 0);
        currentPlayerBall.GetComponent<PlayerMovement>().gameSceneController = this;
    }

    private void Update()
    {
        if (gameState == GameState.WAITING_TO_START)
        {
            WaitingToStart();
        }
        else if (gameState == GameState.GAME)
        {
            Game();

        }
        else if (gameState == GameState.END_GAME)
        {
            EndGame();
        }
    }

    void WaitingToStart()
    {
        Debug.Log("ListLength = " + PhotonNetwork.PlayerList.Length);
        Debug.Log("hasLoaded = " + hasLoaded);
        if (masterClientConnected && !hasIncreased)
        {
            photonView.RPC("IncreaseHasLoaded", RpcTarget.All);
            photonView.RPC("PlayersAlive", RpcTarget.All);
            hasIncreased = true;
        }
        if (PhotonNetwork.PlayerList.Length == hasLoaded && !hasStartedCoroutine)
        {
            //photonView.RPC("StartCouroutineCountDown", RpcTarget.All);
            StartCoroutine("StartCountDown");
            hasStartedCoroutine = true;
        }
    }

    void Game()
    {
        Debug.Log("PlayersAlive " + playerAlive);
        if (playerAlive <= 1)
        {
            Debug.Log("Player List Length in Game " + PhotonNetwork.PlayerList.Length);
            gameState = GameState.END_GAME;
        }
    }

    void EndGame()
    {
        if (currentPlayerBall != null)
        {
            photonView.RPC("WinnerName", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
        }


        //photonView.RPC("StartCouroutineGameFinished", RpcTarget.All);
        if (winnerName != "Nobody")
        {
            photonView.RPC("StartCouroutineGameFinished", RpcTarget.All);
        }
    }

    public void HasDiedOrDisconnected()
    {
        playerAlive -= 1;                                       //Networked
    }

    IEnumerator StartCountDown()
    {
        centralText.text = "Ready?";
        yield return new WaitForSeconds(3f);
        centralText.text = "3";
        yield return new WaitForSeconds(1f);
        centralText.text = "2";
        yield return new WaitForSeconds(1f);
        centralText.text = "1";
        yield return new WaitForSeconds(1f);
        centralText.text = "FIGHT!!!";
        gameState = GameState.GAME;
        yield return new WaitForSeconds(2f);
        centralText.text = "";
    }

    IEnumerator GameFinished()
    {
        centralText.text = winnerName + " won!";
        yield return new WaitForSeconds(4f);
        PhotonNetwork.LoadLevel("WaitingRoom");


    }
    #region PunCallbacks
    /*private void OnConnectedToServer()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("MasterClientConnected", RpcTarget.All);
        }

        Vector3 spawnPosition = new Vector3(0,0,0);


        currentPlayerBall = PhotonNetwork.Instantiate("Ball", spawnPosition, Quaternion.identity, 0);
        currentPlayerBall.GetComponent<PlayerMovement>().gameSceneController = this;
    }*/

    #endregion

    #region PunRPC


    [PunRPC]
    void IncreaseHasLoaded()
    {
        hasLoaded += 1;
    }

    [PunRPC]
    void StartCouroutineCountDown()
    {
        StartCoroutine("StartCountDown");
    }

    [PunRPC]
    void StartCouroutineGameFinished()
    {
        StartCoroutine("GameFinished");
    }

    [PunRPC]
    void MasterClientConnected()
    {
        Debug.Log("RPC");
        //playerAlive = PhotonNetwork.PlayerList.Length;
        Debug.Log("Player list length " + PhotonNetwork.PlayerList.Length);
        hasLoaded = 0;                                      
        masterClientConnected = true;
    }

    [PunRPC]
    void PlayersAlive()
    {
        playerAlive = hasLoaded;
    }

    [PunRPC]
    void WinnerName(string name)
    {

        winnerName = name;
    }
    #endregion


}
