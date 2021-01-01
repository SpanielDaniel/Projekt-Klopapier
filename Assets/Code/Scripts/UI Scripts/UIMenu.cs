//Created by Daniel Pobijanski

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject options;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider seSlider;
    [SerializeField]
    private Slider voiceSlider;
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void BackMenuButton()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void BGMSlider()
    {
        //program me
    }

    public void SESlider()
    {
        //program me
    }

    public void VoideSlider()
    {
        //program me
    }
}
