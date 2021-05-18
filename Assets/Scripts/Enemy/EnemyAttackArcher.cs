using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArcher : MonoBehaviour
{
    public Animator _animator;
    [Header("Attack Settings")]
    public Transform _attackPoint;
    public float _attackRange = 0.5f;
    public LayerMask _playerLayer;
    public int _attackDamage = 10;

    private int _currentAttack = 0;
    private float _timeSinceAttack = 0.0f;

    [SerializeField]
    private GameObject player;
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
        _timeSinceAttack += Time.deltaTime;
        Attack();
    }

    private void Attack()
    {
        enemyScript.LookPlayer();
        // attack animation
        if (_timeSinceAttack > 1f && enemyScript.getChasing())
        {
            
            Debug.Log(enemyScript.getFaceRight());
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + (enemyScript.getFaceRight() ? 1f : -1f), transform.position.y), (enemyScript.getFaceRight() ? Vector2.right : Vector2.left), 10f);
            Debug.DrawRay(new Vector2(transform.position.x + (enemyScript.getFaceRight() ? 1f : -1f), transform.position.y), (enemyScript.getFaceRight() ? Vector2.right : Vector2.left) * 10f, Color.green);
            if (hit.collider != null && hit.collider.gameObject.layer == 9)
            {
                _animator.SetFloat("speedX", 0);
                _animator.SetTrigger("Attack");
                if (!enemyScript.getAttacking())
                {
                    enemyScript.setAttacking(true);
                }
                _timeSinceAttack = 0;
            } else
            {
                if (enemyScript.getAttacking())
                {
                    enemyScript.setAttacking(false);
                }
            }
        }
    }

    private void AE_CheckHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            player.GetComponent<HealthScript>().TakeDamage(_attackDamage);
        }
    }


}
