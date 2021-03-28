using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundLayerMask;
    Rigidbody2D _rb;
    CapsuleCollider2D _capsuleCollider2d;
    public Vector2 _speed = new Vector2(5, 5);
    public float _jumpForce = 2;
    public float _dashSpeed = 5;
    public Animator _animator;
    private float _inputX = 0f;
    private bool _isAttacking = false;
    private bool _isRolling = false;
    private bool _isFaceRight = true;
    private bool _isGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        _groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        Attack();
    }
    void Update()
    {
        Jump();
        Roll();
        //Dash();
    }
    public void Movement()
    {
        //taking input from user
        _inputX = Input.GetAxis("Horizontal");

        _animator.SetFloat("speedX", Mathf.Abs(_inputX));
        if (_inputX != 0 && (!_isAttacking || isGrounded()) && !_isRolling)
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
        if ((isGrounded() && Input.GetButtonDown("Jump") && !_isRolling))
        {
            _animator.SetBool("isJumping", true);

            _rb.velocity = new Vector3(0, _jumpForce, 0);
            // this is for jump animation if it on air
            _animator.SetFloat("speedX", 0);
        }
    }
    private void Roll()
    {
        if (Input.GetButtonDown("Roll") && isGrounded() && Mathf.Abs(_inputX) > 0)
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
    //private void Dash()
    //{
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        if (_inputX > 0)
    //        {
    //            _rb.velocity = Vector2.right * _dashSpeed;
    //        }
    //        else if (_inputX < 0)
    //        {
    //          _rb.velocity = Vector2.left * _dashSpeed;

    //        }            
    //    }
    //}
    private bool isGrounded()
    {
        float extraHeightText = 0.2f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(_capsuleCollider2d.bounds.center, _capsuleCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, _groundLayerMask);
        RaycastHit2D grabCastHit = Physics2D.BoxCast(_capsuleCollider2d.bounds.center, _capsuleCollider2d.bounds.size, _isFaceRight ? 90f : -90f, Vector2.left, extraHeightText, _groundLayerMask);
        Debug.Log(grabCastHit.collider);
        Color rayColor;
        if (raycastHit.collider != null || grabCastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        if (grabCastHit.collider != null)
        {
            _isGrabbing = true;
        }
        else
        {
            _isGrabbing = false;
        }

        Debug.DrawRay(_capsuleCollider2d.bounds.center, new Vector3(_isFaceRight ? .75f : -.75f, 0, 0), rayColor);
        Debug.DrawRay(_capsuleCollider2d.bounds.center + new Vector3(_capsuleCollider2d.bounds.extents.x, 0), Vector2.down * (_capsuleCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(_capsuleCollider2d.bounds.center - new Vector3(_capsuleCollider2d.bounds.extents.x, 0), Vector2.down * (_capsuleCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(_capsuleCollider2d.bounds.center - new Vector3(_capsuleCollider2d.bounds.extents.x, _capsuleCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (_capsuleCollider2d.bounds.extents.x), rayColor);
        if (raycastHit.collider != null && Mathf.Abs(_rb.velocity.y) == 0)
        {
            _animator.SetBool("isJumping", false);
        }
        return (raycastHit.collider != null || grabCastHit.collider != null);
    }
}
