using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    public GameObject playerHero;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHero.GetComponent<CharacterController2D>().SavePlayer();
            Destroy(gameObject);
        }
    }
}
