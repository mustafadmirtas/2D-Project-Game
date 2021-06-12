using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAttack : MonoBehaviour
{
    private Rigidbody2D _rb;
    public Animator _animator;
    [Header("Attack Settings")]
    public Transform _attackPoint;
    public float _attackRange = 0.5f;
    public LayerMask _playerLayer;
    public int _attackDamage = 20;
    public float _jumpAttackRange = 4f;
    public float _jumpAttackRangeMin = 2f;

    private int _currentAttack = 0;
    private float _timeSinceAttack = 1f;
    private float _sinceJumpAttack = 4f;
    private float _sinceDogde = 1f;

    public float _xForce = 5f;
    public float _yForce = 5f;

    private bool _isJumpAttack = false;
    private bool _isDogding = false;

    private SoundManager soundManager;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    private EnemyMovementGround enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        enemyScript = enemy.GetComponent<EnemyMovementGround>();
        _rb = GetComponent<Rigidbody2D>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceAttack += Time.deltaTime;
        _sinceJumpAttack += Time.deltaTime;
        _sinceDogde += Time.deltaTime;
        JumpAttack();
    }
    private void JumpAttack()
    {
        if (Vector2.Distance(player.gameObject.transform.position, transform.position) < _jumpAttackRange && Vector2.Distance(player.gameObject.transform.position, this.transform.position) > _jumpAttackRangeMin && _sinceJumpAttack > 4f && enemyScript.getChasing() && !_isDogding)
        {
            _isJumpAttack = true;
            _sinceJumpAttack = 0f;
            _animator.SetTrigger("JumpAttack");
            _rb.AddForce(new Vector2(enemyScript.getFaceRight() ? _xForce : -_xForce, _yForce), ForceMode2D.Force);
        }
    }
    public void AE_ResetJumpBool()
    {
        _isJumpAttack = false;
    }

    //private void Dogde()
    //{
    //    if (enemyScript.getChasing() && _sinceDogde >= 2f)
    //    {
    //        _sinceDogde = 0f;
    //        _isDogding = true;
    //        _animator.SetTrigger("Roll");
    //        _rb.AddForce(new Vector2(enemyScript.getFaceRight() ? 4f : -4f, 0), ForceMode2D.Force);
    //    }
    //}
    //public void AE_ResetDodge()
    //{
    //    _isDogding = false;
    //}

    private void Attack()
    {
        // attack animation
        if (_timeSinceAttack > 0.75f && !_isJumpAttack && !_isDogding)
        {
            _currentAttack++;

            // Loop back to one after fourth attack
            if (_currentAttack > 4)
                _currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (_timeSinceAttack > 3.0f)
                _currentAttack = 1;

            _animator.SetTrigger("Attack" + _currentAttack);
            // Reset timer
            _timeSinceAttack = 0.0f;
        }
    }

    private void AE_CheckHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (!player.GetComponent<CharacterController2D>()._isBlockState || enemyScript.getFaceRight() == player.GetComponent<CharacterController2D>().getFaceRight())
            {
                player.GetComponent<HealthScript>().TakeDamage(_attackDamage);
            }
        }
    }

    public void swingSounds(int i)
    {
        soundManager.PlaySound("SFX_Swing" + i);
    }

    private void AE_FixDeathAnimation()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.38f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 && this.enabled == true && !_isJumpAttack)
        {
            Attack();
        }
    }

}
