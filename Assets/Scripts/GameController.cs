using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField] Vector3 spawn1;    
    [SerializeField] Vector3 spawn2;    
    [SerializeField] Vector3 spawn3;    
    [SerializeField] Vector3 spawn4;    
    [SerializeField] GameController actualGameSceneController;
    GameObject currentPlayerBall;       
    PlayerMovement playerMovement;
    [SerializeField] TextMeshProUGUI centralText;
    string winnerName = "Nobody";                  
    bool hasIncreased = false;
    bool hasStartedCountDownCoroutine = false;
    bool hasStartedEndCoroutine = false;


    public enum GameState
    {
        WAITING_TO_START,
        GAME,
        END_GAME
    }

    public GameState gameState;            

    bool masterClientLoaded;     

    int hasLoaded;                  

    int playerAlive;                

    private void Start()
    {
        gameState = GameState.WAITING_TO_START;         
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("MasterClientLoaded", RpcTarget.All);
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
        playerMovement = currentPlayerBall.GetComponent<PlayerMovement>();
        playerMovement.gameSceneController = this;
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
        if (masterClientLoaded && !hasIncreased)
        {
            photonView.RPC("IncreaseHasLoaded", RpcTarget.All);
            photonView.RPC("PlayersAlive", RpcTarget.All);
            hasIncreased = true;
        }
        if (PhotonNetwork.PlayerList.Length == hasLoaded && !hasStartedCountDownCoroutine)
        {
            StartCoroutine("StartCountDown");
            hasStartedCountDownCoroutine = true;
        }
    }

    void Game()
    {
        if (playerAlive <= 1)
        {
            Debug.Log("Player List Length in Game " + PhotonNetwork.PlayerList.Length);
            gameState = GameState.END_GAME;
        }
    }

    void EndGame()
    {
        Debug.Log("EndGame");
        if (playerMovement.isAlive == true)
        {
            photonView.RPC("WinnerName", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);

        }

        
        if (winnerName != "Nobody" && !hasStartedEndCoroutine)
        {
            photonView.RPC("StartCouroutineGameFinished", RpcTarget.All);
            hasStartedEndCoroutine = true;
        }
    }

    public void HasDiedOrDisconnected()
    {
        Debug.Log("HasDied");
        photonView.RPC("PlayerDead", RpcTarget.All);                                    
        playerMovement.isAlive = false;
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
    void MasterClientLoaded()
    {
        hasLoaded = 0;                                      
        masterClientLoaded = true;
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

    [PunRPC]
    void PlayerDead()
    {
        playerAlive -= 1;
    }
    #endregion


}
