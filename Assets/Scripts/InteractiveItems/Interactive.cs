using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactive : MonoBehaviour
{
    public GameObject playerHero;
    public GameObject e_key;
    public Sprite openedDoor;
    public Sprite closedDoor;
    public Sprite openedButton;
    public Sprite closedButton;
    public GameObject door;
    private bool isOpened = false;
    private bool playerOnRange = false;
    private void Update()
    {
        OpenOrCloseDoor();
    }

    private void OpenOrCloseDoor()
    {
        if (Input.GetButtonDown("Interaction") && playerOnRange)
        {
            if (isOpened)
            {
                GetComponent<SpriteRenderer>().sprite = closedButton;
                door.GetComponent<SpriteRenderer>().sprite = closedDoor;
                door.GetComponent<LevelFinish>()._isFinishable = false;
                isOpened = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = openedButton;
                door.GetComponent<SpriteRenderer>().sprite = openedDoor;
                door.GetComponent<LevelFinish>()._isFinishable = true;
                isOpened = true;
            }
        }
    }

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            e_key.SetActive(true);
            playerOnRange = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            e_key.SetActive(true);
            playerOnRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        e_key.SetActive(false);
        playerOnRange = false;
    }
}
