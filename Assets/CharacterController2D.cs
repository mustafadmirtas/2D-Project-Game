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
    public bool _onGrounded = true;
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
    }
    public void Movement()
    {
        //taking input from user
        _inputX = Input.GetAxis("Horizontal");

        // creating movement vector
        Vector3 movement = new Vector3(_speed.x * _inputX, 0, 0);
        // 
        movement *= Time.deltaTime;

        // changing position of character
        transform.Translate(movement);

        // Jump movement
        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rb.velocity.y) < 0.001f)
        {
            _animator.SetBool("isJumping", true);
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);

            _animator.SetFloat("speedX", 0);
        }
        // this is for jump animation if it is not air
        if (Mathf.Abs(_rb.velocity.y) < 0.001f)
        {
            _animator.SetFloat("speedX", Mathf.Abs(_inputX));
        }
        // for character rotation
        if (_inputX >= 0.01f)
        {
            transform.localScale = new Vector3(5, 5, 5);
        } else if (_inputX <= -0.01f)
        {
            transform.localScale = new Vector3(-5, 5, 5);
        }
        // attack animation
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("attackEnemy");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
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
