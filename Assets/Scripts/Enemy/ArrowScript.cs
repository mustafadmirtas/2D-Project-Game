using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int _attackDamage = 5;

    private SoundManager soundManager;
    private void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<HealthScript>().TakeDamage(_attackDamage);
            soundManager.PlaySound("SFX_Hit3");
            Destroy(gameObject);
        } else if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }

}
