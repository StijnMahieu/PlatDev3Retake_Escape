using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGameScript : MonoBehaviour
{
    public void FinishGame()
    {
        SceneManager.LoadScene(3);
    }

    private void OnTriggerEnter(Collider _collision)
    {
        if(_collision.gameObject.tag == "MainCharacter")
        {
            FinishGame();
        }
    }
}
