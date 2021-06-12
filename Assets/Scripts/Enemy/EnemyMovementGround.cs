using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementGround : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private GameObject player;
    Rigidbody2D _rb;
    public Vector2 _speed = new Vector2(1, 0);
    public float characterSize = 2.2f;
    public Animator _animator;
    private bool _isAttacking = false;
    private bool _isChasing = false;
    public bool _isFaceRight = true;
    private bool _nearEnemy = false;
    private bool _isTakingHit = false;
    private bool _isBlockState = false;
    private bool _isNoWayToGo = false;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.enabled == true)
        {
            LookPlayer();
        }
    }
    private void FixedUpdate()
    {
        if (this.enabled == true)
        {
            Chase();
            Movement();
            StartChase();
        }
    }
    public void Movement()
    {
        if (!_isBlockState && !_isAttacking && !_isTakingHit && !_isChasing)
        {
            _animator.SetFloat("speedX", 1);
            // creating movement vector
            _rb.velocity = _isFaceRight ? _speed : -_speed;

            // for character rotation
            transform.localScale = new Vector3(_isFaceRight == false ? (-characterSize) : characterSize, characterSize, characterSize);
        }
    }

    public void Chase()
    {
        // follow the player
        if (!_isAttacking && !_isTakingHit && _isChasing && (transform.position.x != player.transform.position.x) && !_isNoWayToGo)
        {
            _rb.velocity = new Vector2(0, 0);
            _animator.SetFloat("speedX", 1);
            transform.localScale = new Vector3(_isFaceRight == false ? (-characterSize) : characterSize, characterSize, characterSize);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 1f * Time.deltaTime);
        }
        else if (_isChasing && (transform.position.x == player.transform.position.x || _isNoWayToGo))
        {
            _animator.SetFloat("speedX", 0);
        }
    }

    public void LookPlayer()
    {
        if (_isChasing || _isAttacking)
        {
            if (transform.position.x > player.transform.position.x && _isFaceRight)
            {
                _isFaceRight = !_isFaceRight;
                transform.localScale = new Vector3(_isFaceRight == false ? (-characterSize) : characterSize, characterSize, characterSize);

            }
            else if (transform.position.x < player.transform.position.x && !_isFaceRight)
            {
                _isFaceRight = !_isFaceRight;
                transform.localScale = new Vector3(_isFaceRight == false ? (-characterSize) : characterSize, characterSize, characterSize);
            }
        }
    }

    public void StartChase()
    {
        // if line detect player start chase
        if (!_isChasing)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + (_isFaceRight ? 1f : -1f), transform.position.y), (_isFaceRight ? Vector2.right : Vector2.left), 10f);
            Debug.DrawRay(new Vector2(transform.position.x + (_isFaceRight ? 1f : -1f), transform.position.y - (characterSize > 2.2f ? 0.3f : 0)), (_isFaceRight ? Vector2.right : Vector2.left) * 10f, Color.green);
            if (hit.collider != null && hit.collider.gameObject.layer == 9)
            {
                _isChasing = true;
            }
        }
    }

    public void AE_TakingDamage()
    {
        _isChasing = true;
        _isTakingHit = true;
    }
    public void AE_TakingDamageFinished()
    {
        _isTakingHit = false;
        _isAttacking = false;
    }
    public void AE_TakingDamageFinishedArcher()
    {
        _isTakingHit = false;
    }

    public void AE_AttackStarted()
    {
        _isAttacking = true;
        _isChasing = true;
        if (enabled)
        {
            _rb.velocity = new Vector2(0, 0);
        }
    }
    public void AE_AttackFinished()
    {
        _isAttacking = false;
    }

    public bool getAttacking()
    {
        return _isAttacking;
    }
    public void setAttacking(bool isAttacking)
    {
        if (_isAttacking != isAttacking)
        {
            _isAttacking = isAttacking;
        }
    }
    public bool getFaceRight()
    {
        return _isFaceRight;
    }
    public void setFaceRight(bool isFaceRight)
    {
        if (_isFaceRight != isFaceRight)
        {
            _isFaceRight = isFaceRight;
        }
    }
    public bool getChasing()
    {
        return _isChasing;
    }
    public void setChasing(bool isChasing)
    {
        if (_isChasing != isChasing)
        {
            _isChasing = isChasing;
        }
    }

    public bool getNoWayToGo()
    {
        return _isNoWayToGo;
    }
    public void setNoWayToGo(bool isNoWayToGo)
    {
        if (_isNoWayToGo != isNoWayToGo)
        {
            _isNoWayToGo = isNoWayToGo;
        }
    }
}
