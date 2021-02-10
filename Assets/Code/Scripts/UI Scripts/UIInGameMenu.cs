using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject MenuButton;

    public void InGameMenu()
    {
        MenuButton.SetActive(true);
        //ToDo: GameSpeed = 0;
    }

    public void Resume()
    {
        MenuButton.SetActive(false);
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
