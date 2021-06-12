using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollactableScript : MonoBehaviour
{
    public GameObject playerHero;
    public GameObject e_key;
    private bool playerOnRange = false;

    // Update is called once per frame
    void Update()
    {
        Collect();
    }

    private void Collect()
    {
        if (Input.GetButtonDown("Interaction") && playerOnRange)
        {
            playerHero.GetComponent<CharacterController2D>()._potionCount += 1;
            playerHero.GetComponent<CharacterController2D>()._potionCountText.text = "" + playerHero.GetComponent<CharacterController2D>()._potionCount;
            playerHero.GetComponent<CharacterController2D>().potion.GetComponent<Image>().sprite = playerHero.GetComponent<CharacterController2D>().potionTextureFill;
            Destroy(gameObject);
        }
    }
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
