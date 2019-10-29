using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] float force;
    [SerializeField] float stopForce;
    [SerializeField] float jumpForce;
    [SerializeField] Rigidbody playerRigidbody;


    void Update()
    {
        /*if (photonView.IsMine == false && PhotonNetwork.IsConnected == false)
        {
            photonView.RPC("ChangeColor", RpcTarget.All);
            //Comment préciser qu'on parle du joueur qui envoie

            return;
        }
        if (photonView.IsMine)
        {

            #region Movements
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (playerRigidbody.velocity.z < 0)
                {
                    playerRigidbody.AddForce(Vector3.forward * stopForce);
                }
                else
                {
                    playerRigidbody.AddForce(Vector3.forward * force);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (playerRigidbody.velocity.z > 0)
                {
                    playerRigidbody.AddForce(Vector3.back * stopForce);
                }
                else
                {
                    playerRigidbody.AddForce(Vector3.back * force);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (playerRigidbody.velocity.x > 0)
                {
                    playerRigidbody.AddForce(Vector3.left * stopForce);
                }
                else
                {
                    playerRigidbody.AddForce(Vector3.left * force);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (playerRigidbody.velocity.x < 0)
                {
                    playerRigidbody.AddForce(Vector3.right * stopForce);
                }
                else
                {
                    playerRigidbody.AddForce(Vector3.right * force);
                }
            }
            #endregion

            #region Player Death
            if (gameObject.transform.position.y <= -10 || gameObject.transform.position.y >= 100)
            {
                PlayerDie();
            }
            #endregion
        }


    }

    void PlayerDie()
    {
        PhotonNetwork.Destroy(gameObject);
        //photonView.RPC("RemoveOnePlayer", RpcTarget.All);
    }

    [PunRPC]
    void ChangeColor()
    {

    }

    void RemoveOnePlayer()
    {
        //PhotonNetwork.Destroy(gameObject);
    }
}*/
