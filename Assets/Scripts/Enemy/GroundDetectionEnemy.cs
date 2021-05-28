using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectionEnemy : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private GameObject enemy;
    private EnemyMovementGround enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        enemyScript = enemy.GetComponent<EnemyMovementGround>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            enemyScript.setNoWayToGo(true);
            enemyScript.setFaceRight(!enemyScript.getFaceRight());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            enemyScript.setNoWayToGo(false);
        }
    }
}
