using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackKnight : MonoBehaviour
{
    public Animator _animator;
    [Header("Attack Settings")]
    public Transform _attackPoint;
    public float _attackRange = 0.5f;
    public LayerMask _playerLayer;
    public int _attackDamage = 10;

    private int _currentAttack = 0;
    private float _timeSinceAttack = 1f;

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
    }

    private void Attack()
    {
        // attack animation
        if (_timeSinceAttack > 1f)
        {
            _animator.SetTrigger("attackEnemy");
            // Reset timer
            _timeSinceAttack = 0.0f;
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 && this.enabled == true)
        {
            Attack();
        }
    }

}
