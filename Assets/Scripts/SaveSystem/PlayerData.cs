using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level = 0;
    public int health;
    public int damage = 10;
    public float[] position = {0, 0, 0};
    
    public PlayerData(GameObject character, List<GameObject> enemies)
    {
        health = character.GetComponent<HealthScript>().getHealth();
        damage = character.GetComponent<CharacterController2D>()._attackDamage;
        position[0] = character.transform.position.x;
        position[1] = character.transform.position.y;
        position[2] = character.transform.position.z;
        level = 1;
    }

}
