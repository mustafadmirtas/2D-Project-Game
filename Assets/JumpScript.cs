using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    public Animator _animator;
    public Rigidbody2D _rb;
    public float _inputX;
    public CharacterController2D cc2D;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        _inputX = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && Mathf.Abs(_inputX) >= 0 && Mathf.Abs(_rb.velocity.y) > 0.001f)
        {
            _animator.SetBool("isJumping", false);
        }
    }
}
