using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuScript : MonoBehaviour
{
    private SceneManagement sceneManager;
    private PlayerData playerData;

    public GameObject[] items;
    public GameObject mainCanvas, optionsCanvas;
    public Toggle fullscreenToogle;
    public Slider soundSlider;
    public TextMeshProUGUI soundPercentage;
    // Start is called before the first frame update
    void Start()
    {
        fullscreenToogle.SetIsOnWithoutNotify(Screen.fullScreen);
        sceneManager = new SceneManagement();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartButton()
    {
        PlayerPrefs.SetInt("Loaded", 0);
        sceneManager.LoadScene(1);
    }
    public void LoadButton()
    {
        playerData = SaveLoad.LoadData();
        PlayerPrefs.SetInt("Loaded", 1);
        sceneManager.LoadScene(playerData.level);
    }
    public void OptionsButton()
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
        optionsCanvas.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    public void SoundSliderChanged()
    {
        soundPercentage.text = "%" + (int)soundSlider.value;
    }
    public void FullScreenModeChanged()
    {
        if (fullscreenToogle.isOn)
        {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("FullScreenMode", 1);
        } else
        {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("FullScreenMode", 0);
        }
    }
    public void DifficultyChanged()
    {

    }
    public void TurnBackMainMenu()
    {
        foreach (GameObject item in items)
        {
            item.SetActive(true);
        }
        optionsCanvas.SetActive(false);
    }
}
