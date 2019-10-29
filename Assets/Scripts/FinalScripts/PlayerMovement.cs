using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] float force;
    [SerializeField] float stopForce;
    [SerializeField] float jumpForce;
    [SerializeField] float baseMass;
    [SerializeField] float xPressedMass;
    [SerializeField] float downDeleteDistance;
    [SerializeField] float upDeleteDistance;

    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] SphereCollider playerCollider;

    public GameSceneController gameSceneController;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (gameSceneController != null)
        {
            if (gameSceneController.gameState == GameSceneController.GameState.WAITING_TO_START || gameSceneController.gameState == GameSceneController.GameState.END_GAME)
            {
                photonView.RPC("ToggleGravity", RpcTarget.All, false);
                playerRigidbody.velocity = new Vector3(0, 0, 0);
            }
            else
            {
                photonView.RPC("ToggleGravity", RpcTarget.All, true);
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

                    #region Special Controls

                    if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
                    {
                        playerRigidbody.AddForce(Vector3.up * jumpForce);
                    }


                    

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        playerRigidbody.mass = xPressedMass;        //RPC
                    }
                    else if (Input.GetKeyUp(KeyCode.X))
                    {
                        playerRigidbody.mass = baseMass;            //RPC
                    }


                    #endregion

                    #region Events
                    if (transform.position.y > upDeleteDistance || transform.position.y < downDeleteDistance)
                    {
                        gameSceneController.HasDiedOrDisconnected();
                    }
                    #endregion
                }
            }
        }

    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z), playerCollider.radius * 0.9f, groundLayers);
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        gameSceneController.HasDiedOrDisconnected();
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (gameSceneController.gameState == GameSceneController.GameState.GAME)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(playerRigidbody.position);
                stream.SendNext(playerRigidbody.velocity);
            }
            else
            {
                playerRigidbody.position = (Vector3)stream.ReceiveNext();
                playerRigidbody.velocity = (Vector3)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
                playerRigidbody.position += playerRigidbody.velocity * lag;
            }
        }
    }

    [PunRPC]
    void IncreaseMass(Rigidbody playerRB)
    {
        playerRB.mass = xPressedMass;
    }

    [PunRPC]
    void DecreaseMass(Rigidbody playerRB)
    {
        playerRB.mass = baseMass;
    }

    [PunRPC]
    void ToggleGravity(bool value)
    {
        playerRigidbody.useGravity = value;
    }
}
