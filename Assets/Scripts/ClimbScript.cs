using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbScript : MonoBehaviour
{
    CharacterController2D cc2d;
    public GameObject character;
    [SerializeField]
    public void Start()
    {
        cc2d = character.GetComponent<CharacterController2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer + " " + LayerMask.NameToLayer("Climb"));
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
    
            cc2d.isClimb(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            cc2d.isClimb(false);
        }
    }
}
