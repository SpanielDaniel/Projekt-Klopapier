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
        FindObjectOfType<AudioManager>().PlayMusic("Music_Track01");
    }

    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------

    #region Buttons

    public void ButtonClicked_Start()
    {
        AudioManager.GetInstance.PlaySound("BuildSlotClicked");
        GameManager.GetInstance.LoadScene(2);
    }

    public void ButtonClicked_Settings()
    {
        AudioManager.GetInstance.PlaySound("BuildSlotClicked");
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void ButtonClicked_Quit()
    {
        AudioManager.GetInstance.PlaySound("BuildSlotClicked");
        GameManager.GetInstance.QuitGame();
    }

    public void ButtonClicked_BackToMainMenu()
    {
        AudioManager.GetInstance.PlaySound("BuildSlotClicked");
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void OnSoundValueChanged(float _value)
    {
        AudioManager.GetInstance.OnSoundValueChanged(_value);
    }

    #endregion
    
    // -------------------------------------------------------------------------------------------------------------
}
