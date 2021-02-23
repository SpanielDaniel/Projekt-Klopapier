//Created by Daniel Pobijanski

using Code.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------

    #region Init

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject OptionsMenu;

    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
   
    private void Start()
    {
        StartMenuMusic();
    }

    
    // -----------------------------------------------------------------------------------------------------------------

    #region Functions
    
    // Start -----------------------------------------------------------------------------------------------------------
    private void StartMenuMusic()
    {
        FindObjectOfType<AudioManager>().Play("MenuMusic");
    }

    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------

    #region Buttons

    public void ButtonClicked_Start()
    {
        AudioManager.GetInstance.Play("BuildSlotClicked");
        GameManager.GetInstance.LoadScene(2);
    }

    public void ButtonClicked_Settings()
    {
        AudioManager.GetInstance.Play("BuildSlotClicked");
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void ButtonClicked_Quit()
    {
        AudioManager.GetInstance.Play("BuildSlotClicked");
        GameManager.GetInstance.QuitGame();
    }

    public void ButtonClicked_BackToMainMenu()
    {
        AudioManager.GetInstance.Play("BuildSlotClicked");
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    #endregion
    
    // -------------------------------------------------------------------------------------------------------------
}
