using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int _maxHealth = 100;
    int _currentHealth;
    public Animator _animator;
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Takes Damage
        _currentHealth -= damage;
        _animator.SetTrigger("Hit");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        if (this.enabled)
        {

            _animator.SetTrigger("Death");
            Collider2D[] collider2Ds = gameObject.GetComponents<Collider2D>();
            foreach (Collider2D collider in collider2Ds)
            {
                collider.enabled = false;
            }
            MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            this.enabled = false;
        }
    }

}
