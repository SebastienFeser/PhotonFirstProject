using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformController : MonoBehaviour
{
    [SerializeField] float plateformDecreasingMultiplier;
    [SerializeField] GameSceneController gameSceneController;

    private void FixedUpdate()
    {
        if (gameSceneController.gameState == GameSceneController.GameState.GAME)
        {
            gameObject.transform.localScale -= new Vector3(plateformDecreasingMultiplier / 50, 0, plateformDecreasingMultiplier / 50);
            if (gameObject.transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
