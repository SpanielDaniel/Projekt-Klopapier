using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject InGameMenuUI;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;

    private void Awake()
    {
        GameManager.WonWindow += ShowWin;
        GameManager.LostWindow += ShowLose;
    }

    public void InGameMenu()
    {
        InGameMenuUI.SetActive(true);
        //ToDo: GameSpeed = 0;
    }

    public void Resume()
    {
        InGameMenuUI.SetActive(false);
        //ToDo: GameSpeed = 1;
    }

    public void ShowWin()
    {
        WinScreen.SetActive(true);
        Invoke("ReturnToMenu", 5f);
    }

    public void ShowLose()
    {
        LoseScreen.SetActive(true);
        Invoke("ReturnToMenu", 5f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
