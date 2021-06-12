using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    public GameObject playerHero;
    public bool _isFinishable = false;
    private bool playerOnRange;
    public GameObject e_key;
    private PlayerData playerData;

    private SceneManagement sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = new SceneManagement();
    }

    // Update is called once per frame
    void Update()
    {
        NextLevel();
    }

    private void NextLevel()
    {
        if (Input.GetButtonDown("Interaction") && playerOnRange && _isFinishable)
        {
            playerData = SaveLoad.LoadData();  
            PlayerPrefs.SetInt("Loaded", 0);
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 1) + 1);
            sceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel", 1));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isFinishable)
        {
            e_key.SetActive(true);
            playerOnRange = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isFinishable)
        {
            playerOnRange = true;
            e_key.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerOnRange = false;
        e_key.SetActive(false);
    }
}
