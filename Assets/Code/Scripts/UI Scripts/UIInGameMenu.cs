using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject InGameMenuUI;

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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
