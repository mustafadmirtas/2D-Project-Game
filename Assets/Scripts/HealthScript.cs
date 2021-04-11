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
        Debug.Log(damage);
        // Takes Damage
        _currentHealth -= damage;
        _animator.SetTrigger("hit");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("died");
        _animator.SetTrigger("death");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

}
