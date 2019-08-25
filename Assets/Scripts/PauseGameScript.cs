using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGameScript : SceneManagerScript
{
    public bool GameIsPaused = false;

    [SerializeField]
    private GameObject _pauseMenuUI;
    [SerializeField]
    private GameObject _inGameUI;

    void Update()
    {
        CheckIfPaused();
    }

    private void CheckIfPaused()
    {
        if (Input.GetButtonDown("StartButton"))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ResumeGame()
    {
        _pauseMenuUI.SetActive(false);
        _inGameUI.SetActive(true);

        //resume gameplay
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    private void PauseGame()
    {
        _pauseMenuUI.SetActive(true);
        _inGameUI.SetActive(false);

        //pauses gameplay
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }
}

