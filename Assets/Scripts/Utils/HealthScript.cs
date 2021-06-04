using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int _maxHealth = 100;
    int _currentHealth;
    public Animator _animator;
    public Slider slider;
    public GameObject healtBarPos;
    public GameObject healthBar;

    void Start()
    {
        _currentHealth = _maxHealth;

    }
    private void Update()
    {
        if (gameObject.layer != 9)
        {
            Vector3 wantedPos = Camera.main.WorldToScreenPoint(healtBarPos.transform.position);
            slider.transform.position = wantedPos;
        }

    }
    public void TakeDamage(int damage)
    {
        // Takes Damage
        _currentHealth -= damage;
        if (slider != null)
        {
            slider.value = _currentHealth;
        }
        _animator.SetTrigger("Hit");

        if (_currentHealth <= 0)
        {
            if (gameObject.tag == "Player")
            {
                BroadcastMessage("Continue");
            }
            Die();
        }
    }
    void Die()
    {
        if (enabled)
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

            if (healthBar != null)
            {
                healthBar.SetActive(false);

            }
            enabled = false;
        }
    }

    public int getHealth()
    {
        return _currentHealth;
    }
    public void setHealth(int health)
    {
        if (_currentHealth != health)
        {
            _currentHealth = health;
            slider.value = _currentHealth;
        }
    }

}
