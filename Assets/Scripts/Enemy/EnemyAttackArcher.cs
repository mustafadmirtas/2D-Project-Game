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
    public GameObject _arrow;
    public float _arrowSpeed = 2f;

    private float _timeSinceAttack = 2f;

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
        if (_timeSinceAttack > 2f && enemyScript.getChasing())
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(new Vector2(transform.position.x + (enemyScript.getFaceRight() ? 0.1f : -0.1f), transform.position.y), (enemyScript.getFaceRight() ? Vector2.right : Vector2.left), 10f);
            Debug.DrawRay(new Vector2(transform.position.x + (enemyScript.getFaceRight() ? 0.1f : -0.1f), transform.position.y - 0.3f), (enemyScript.getFaceRight() ? Vector2.right : Vector2.left) * 10f, Color.green);
            foreach (RaycastHit2D rays in hit)
            {
                if (rays.collider != null && rays.collider.gameObject.layer == 9)
                {
                    _animator.SetFloat("speedX", 0);
                    _animator.SetTrigger("Attack");
                    if (!enemyScript.getAttacking())
                    {
                        enemyScript.setAttacking(true);
                    }
                }
            }

            _timeSinceAttack = 0;
        }
    }

    private void ShotArrow()
    {
        //Creating Arrow
        float x = transform.position.x + (enemyScript.getFaceRight() ? 0.4f : -0.4f);
        float y = transform.position.y - 0.22f;
        GameObject arrow = Instantiate(_arrow, new Vector2(x, y), Quaternion.identity);
        arrow.transform.localScale = new Vector3(enemyScript.getFaceRight() ? 0.1f : -0.1f, 0.1f, 0.1f);
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(enemyScript.getFaceRight() ? _arrowSpeed : -_arrowSpeed, 0f);

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
