using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    private SceneManagement sceneManager;
    private PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = new SceneManagement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
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

    }

    public void ExitButton()
    {

    }
}
