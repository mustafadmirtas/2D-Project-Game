using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuScript : MonoBehaviour
{
    private SceneManagement sceneManager;
    private PlayerData playerData;
    private SoundManager soundManager;

    public AudioSource themeAudioSource;
    public AudioSource effectAudioSource;

    public GameObject[] items;
    public Button continueButton;
    public GameObject mainCanvas, optionsCanvas, newGameCanvas;
    public Toggle fullscreenToogle;
    public Slider soundSlider;
    public Slider themeSoundSlider;
    public TextMeshProUGUI soundPercentage;
    public TextMeshProUGUI effectSoundPercentage;
    public TMP_Dropdown dropDownDiff, dropDownDiff2;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GetComponent<SoundManager>();
        sceneManager = new SceneManagement();
        fullscreenToogle.SetIsOnWithoutNotify(Screen.fullScreen);

        GetSoundData();

        playerData = SaveLoad.LoadData();
        if (playerData == null)
        {
            continueButton.enabled = false;
            continueButton.image.enabled = false;
            continueButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void NewGameButton()
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
        newGameCanvas.SetActive(true);
    }
    public void StartButton()
    {

        PlayerPrefs.SetInt("Loaded", 0);
        sceneManager.LoadScene(1);
        soundManager.PlaySound("SFX_Click");
    }
    public void LoadButton()
    {
        soundManager.PlaySound("SFX_Click");
        playerData = SaveLoad.LoadData();
        PlayerPrefs.SetInt("Loaded", 1);
        sceneManager.LoadScene(playerData.level);
    }
    public void OptionsButton()
    {
        soundManager.PlaySound("SFX_Click");
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
        optionsCanvas.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        sceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        soundManager.PlaySound("SFX_Click");
        Application.Quit();
    }
    public void EffectSoundSliderChanged()
    {
        effectSoundPercentage.text = "%" + (int)soundSlider.value;
        effectAudioSource.volume = (soundSlider.value / 100);
        PlayerPrefs.SetFloat("EffectSoundVolume", effectAudioSource.volume);
    }
    public void ThemeSoundSliderChanged()
    {
        soundPercentage.text = "%" + (int)themeSoundSlider.value;
        themeAudioSource.volume = (themeSoundSlider.value / 100);
        PlayerPrefs.SetFloat("ThemeSourceVolume", themeAudioSource.volume);
    }

    private void GetSoundData()
    {
        soundSlider.value = PlayerPrefs.GetFloat("EffectSoundVolume", 1) * 100;
        themeSoundSlider.value = PlayerPrefs.GetFloat("ThemeSourceVolume", 1) * 100;
    }

    public void FullScreenModeChanged()
    {
        if (fullscreenToogle.isOn)
        {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("FullScreenMode", 1);
        }
        else
        {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("FullScreenMode", 0);
        }
    }
    public void DifficultyChanged(TMP_Dropdown dropdown)
    {
        soundManager.PlaySound("SFX_Click");
        PlayerPrefs.SetInt("Difficulty", dropdown.value);
        dropDownDiff.value = dropdown.value;
        if (dropDownDiff2 != null)
        {
            dropDownDiff2.value = dropdown.value;
        }
    }

    public void TurnBackMainMenu()
    {
        soundManager.PlaySound("SFX_Click");

        foreach (GameObject item in items)
        {
            item.SetActive(true);
        }
        optionsCanvas.SetActive(false);
        if (newGameCanvas != null)
        {
            newGameCanvas.SetActive(false);
        }
    }
}
