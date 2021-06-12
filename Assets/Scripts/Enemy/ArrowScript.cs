using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int _attackDamage = 10;
    public bool _isFaceRight;

    private SoundManager soundManager;
    private GameObject player;
    private void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 && !player.GetComponent<CharacterController2D>()._isBlockState || _isFaceRight == player.GetComponent<CharacterController2D>().getFaceRight())
        {
            collision.gameObject.GetComponent<HealthScript>().TakeDamage(_attackDamage);
            Destroy(gameObject);
        } else if (collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
        } else if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }

}
