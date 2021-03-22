using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D _rb;
    public Vector2 _speed = new Vector2(1, 1);
    public float _jumpForce = 2;
    public Animator _animator;
    private float _inputX = 0f;
    private bool _onGrounded = true;
    private bool _isAttacking = false;
    private bool _isRolling = false;
    private bool _isFaceRight = true;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
        Roll();
        Attack();
    }
    public void Movement()
    {
        //taking input from user
        _inputX = Input.GetAxis("Horizontal");

        _animator.SetFloat("speedX", Mathf.Abs(_inputX));
        if (_inputX != 0 && (!_isAttacking || !_onGrounded) && !_isRolling)
        {
            // creating movement vector
            Vector3 movement = new Vector3(_speed.x * _inputX, 0, 0);
            // 
            movement *= Time.deltaTime;

            // for character rotation
            transform.localScale = new Vector3(_inputX < 0 ? (-5) : 5, 5, 5);
            _isFaceRight = _inputX < 0 ? false : true;
            // changing position of character
            transform.Translate(movement);
        }
    }
    private void Jump()
    {
        // Jump movement
        if (Input.GetButtonDown("Jump") && _onGrounded && !_isRolling)
        {
            _animator.SetBool("isJumping", true);
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            // this is for jump animation if it on air
            _animator.SetFloat("speedX", 0);
        }
    }
    private void Roll()
    {
        if (Input.GetButtonDown("Roll") && _onGrounded && Mathf.Abs(_inputX) > 0)
        {
            _isRolling = true;
            _animator.SetTrigger("roll");
        }

    }

    private void Attack()
    {
        // attack animation
        if (Input.GetButtonDown("Fire1"))
        {
            _isAttacking = true;
            _animator.SetTrigger("attackEnemy");
        }
    }

    private void AttackComplete()
    {
        _isAttacking = false;
    }

    private void RollComplete()
    {
        _isRolling = false;
    }
    private void RollStart()
    {
        _rb.AddForce(new Vector2(_isFaceRight ? 5f : -5f, 0), ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && !_onGrounded)
        {
            _animator.SetBool("isJumping", false);
            _onGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _onGrounded = false;
        }
    }
}
