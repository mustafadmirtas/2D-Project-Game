using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level = 0;
    public int health;
    public int damage = 10;
    public float[] position = { 0, 0, 0 };
    public float[] cameraPos = { 0, 0, 0 };

    public List<int> enemyHealths = new List<int>();
    public List<bool> healthEnabled = new List<bool>();
    public List<float[]> enemyPos = new List<float[]>();
    public List<bool> enemyEnabled = new List<bool>();
    public PlayerData(GameObject character, List<GameObject> enemies, GameObject camera)
    {
        health = character.GetComponent<HealthScript>().getHealth();
        damage = character.GetComponent<CharacterController2D>()._attackDamage;
        position[0] = character.transform.position.x;
        position[1] = character.transform.position.y;
        position[2] = character.transform.position.z;
        level = 1;

        cameraPos[0] = camera.transform.position.x;
        cameraPos[1] = camera.transform.position.y;
        cameraPos[2] = camera.transform.position.z;

        Debug.Log(enemies.Count + "asda");
        foreach (GameObject enemy in enemies)
        {
            healthEnabled.Add(enemy.GetComponent<HealthScript>().enabled);
            if (enemy.GetComponent<HealthScript>().enabled)
            {
                Debug.Log(enemy.GetComponent<HealthScript>().getHealth());
                enemyHealths.Add(enemy.GetComponent<HealthScript>().getHealth());
            } else
            {
                enemyHealths.Add(0);
            }
            enemyPos.Add(new float[] { enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z });
            enemyEnabled.Add(enemy.activeSelf);            
        }
    }

}
