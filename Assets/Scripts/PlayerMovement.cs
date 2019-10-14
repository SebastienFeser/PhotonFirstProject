using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] float stopForce;
    [SerializeField] float jumpForce;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] PhotonView photonView;


    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == false)
        {
            return;
        }

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

        #region
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce);
        }
        #endregion
    }
}
