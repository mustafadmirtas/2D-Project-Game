using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundLayerMask;
    Rigidbody2D _rb;
    CapsuleCollider2D _capsuleCollider2D;
    public Vector2 _speed = new Vector2(1, 1);
    public float _jumpForce = 5;
    public float _rollForce = 7.5f;
    public float _dashSpeed = 5;
    public Animator _animator;
    private float _inputX = 0f;
    private bool _isAttacking = false;
    private bool _isRolling = false;
    private bool _isFaceRight = true;
    private bool _isGrabbing = false;
    private int _currentAttack = 0;
    private float _timeSinceAttack = 0.0f;
    private bool _isClimbing = false;
    public bool _isBlockState = false;
    public int _healthSteal = 0;

    [Header("Attack Settings")]
    public Transform _attackPoint;
    public float _attackRange = 0.5f;
    public LayerMask _enemyLayer;
    public int _attackDamage = 10;

    [Header("Load Settings")]
    public GameObject _camera;
    public bool _isLoaded = false;
    private GameObject[] gos;
    private List<GameObject> goList;
    private SoundManager soundManager;
    public GameObject inGameCanvas;

    [Header("Potion")]
    public int _potionCount = 3;
    public TextMeshProUGUI _potionCountText;
    public Sprite potionTextureEmpty;
    public Sprite potionTextureFill;
    public GameObject potion;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _groundLayerMask = LayerMask.GetMask("Ground");
        if (PlayerPrefs.GetInt("Loaded") == 1)
        {
            LoadPlayer();
        }
        _potionCountText.text = ""+_potionCount;
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        Climb();
    }
    void Update()
    {
        if (Time.timeScale > 0)
        {
            _timeSinceAttack += Time.deltaTime;
            Jump();
            Roll();
            Attack();
            Block();
            Dash();
            DrinkPotion();
        }
        PauseOrPlayGame(false);
    }
    public void Movement()
    {
        //taking input from user
        _inputX = Input.GetAxis("Horizontal");

        if (isGrounded())
        {
            _animator.SetFloat("speedX", Mathf.Abs(_inputX));
        }
        _animator.SetFloat("AirSpeedY", _rb.velocity.y);
        if (_inputX != 0 && !_isRolling && !_isBlockState)
        {
            // creating movement vector
            Vector3 movement = new Vector3(_speed.x * _inputX, 0, 0);
            // 
            movement *= Time.deltaTime;

            // for character rotation
            transform.localScale = new Vector3(_inputX < 0 ? (-1) : 1, 1, 1);
            _isFaceRight = _inputX < 0 ? false : true;
            // changing position of character
            transform.Translate(movement);
        }
    }
    private void Jump()
    {
        // Jump movement
        if (((isGrounded() || _isClimbing) && Input.GetButtonDown("Jump") && !_isRolling))
        {
            _isClimbing = false;
            _rb.gravityScale = 1f;
            _animator.SetBool("Grounded", false);
            _animator.SetTrigger("Jump");

            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            // this is for jump animation if it on air
            _animator.SetFloat("speedX", 0);
        }
    }
    private void Roll()
    {
        if (Input.GetButtonDown("Roll") && isGrounded() && Mathf.Abs(_inputX) > 0)
        {
            _isRolling = true;
            _animator.SetTrigger("Roll");
            _rb.velocity = new Vector2(((_isFaceRight ? 1 : -1) * _rollForce), _rb.velocity.y);
        }

    }

    private void Attack()
    {
        // attack animation
        if (Input.GetButtonDown("Fire1") && _timeSinceAttack > 0.25f && !_isRolling)
        {
            _isAttacking = true;
            _currentAttack++;

            // Loop back to one after third attack
            if (_currentAttack > 3)
                _currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (_timeSinceAttack > 1.0f)
                _currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            _animator.SetTrigger("Attack" + _currentAttack);

            // Reset timer
            _timeSinceAttack = 0.0f;
        }
    }

    private void Block()
    {
        if (Input.GetButtonDown("Block"))
        {
            _isBlockState = true;
            _animator.SetBool("IdleBlock", true);
        }
        if (Input.GetButtonUp("Block"))
        {
            _isBlockState = false;
            _animator.SetBool("IdleBlock", false);
        }
    }
    private void DrinkPotion()
    {
        if (Input.GetButtonDown("PotionHeal") && _potionCount > 0)
        {
            if (GetComponent<HealthScript>().getHealth() == 100)
            {
                return;
            }
            soundManager.PlaySound("SFX_DrinkPot");
            if (GetComponent<HealthScript>().getHealth() > 80)
            {
                GetComponent<HealthScript>().setHealth(100);

            } else
            {
                GetComponent<HealthScript>().setHealth(GetComponent<HealthScript>().getHealth() + 20);
            }
            _potionCount--;
            _potionCountText.text = "" + _potionCount;
            if (_potionCount == 0)
            {
                potion.GetComponent<Image>().sprite = potionTextureEmpty;
            }
        }
    }
    private void Dash()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    if (_inputX > 0)
        //    {
        //        _rb.velocity = Vector2.right * _dashSpeed;
        //    }
        //    else if (_inputX < 0)
        //    {
        //        _rb.velocity = Vector2.left * _dashSpeed;

        //    }
        //}
    }
    private bool isGrounded()
    {
        float extraHeightText = 0.2f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(_capsuleCollider2D.bounds.center, _capsuleCollider2D.bounds.size, 0f, Vector2.down, extraHeightText, _groundLayerMask);
        RaycastHit2D grabCastHit = Physics2D.BoxCast(_capsuleCollider2D.bounds.center, _capsuleCollider2D.bounds.size, _isFaceRight ? 90f : -90f, Vector2.left, extraHeightText, _groundLayerMask);
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

        Debug.DrawRay(_capsuleCollider2D.bounds.center, new Vector3(_isFaceRight ? .75f : -.75f, 0, 0), rayColor);
        Debug.DrawRay(_capsuleCollider2D.bounds.center + new Vector3(_capsuleCollider2D.bounds.extents.x, 0), Vector2.down * (_capsuleCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(_capsuleCollider2D.bounds.center - new Vector3(_capsuleCollider2D.bounds.extents.x, 0), Vector2.down * (_capsuleCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(_capsuleCollider2D.bounds.center - new Vector3(_capsuleCollider2D.bounds.extents.x, _capsuleCollider2D.bounds.extents.y + extraHeightText), Vector2.right * (_capsuleCollider2D.bounds.extents.x), rayColor);
        if (raycastHit.collider != null && Mathf.Abs(_rb.velocity.y) == 0)
        {
            _animator.SetBool("Grounded", true);
        }
        else
        {
            _animator.SetBool("Grounded", false);
        }
        return (raycastHit.collider != null || grabCastHit.collider != null);
    }
    public void isClimb(bool climbing)
    {
        if (climbing)
        {
            _rb.gravityScale = 0;
            _rb.velocity = new Vector2(0f, 0f);
            _isClimbing = true;
        }
        else
        {
            _rb.gravityScale = 1;
            _isClimbing = false;
        }
    }
    private void Climb()
    {
        float _inputY = Input.GetAxis("Vertical");
        if (_isClimbing)
        {
            Vector3 movement = new Vector3(_rb.velocity.x, _speed.y * (_inputY / 2), 0);
            // 
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }
    }
    private void OnDrawGizmosSelected()
    {
        // for seeing sphere which gives damage
        if (_attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
    // Animation Events
    private void AE_ResetRoll()
    {
        _isRolling = false;
    }
    private void AE_CheckHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<HealthScript>().TakeDamage(_attackDamage);
            soundManager.PlaySound("SFX_Hit1");
            GetComponent<HealthScript>().setHealth(GetComponent<HealthScript>().getHealth() + _healthSteal);
        }
    }
    // Sound Functions
    public void swingSounds(int i)
    {
        soundManager.PlaySound("SFX_Swing" + i);
    }
    public void runSound()
    {
        soundManager.PlaySound("SFX_Walk");
    }
    // Gettter Setter
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
    // Load Save Functions
    public void SavePlayer()
    {
        SaveLoad.SaveData(gameObject, findEnemies(), _camera);
    }

    private List<GameObject> findEnemies()
    {

        gos = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        goList = new List<GameObject>();
        foreach (GameObject go in gos)
        {
            if (go.layer == 8)
            {
                goList.Add(go);
            }
        }
        return goList;
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveLoad.LoadData();
        GetComponent<HealthScript>().setHealth(data.health);
        _attackDamage = data.damage;
        _potionCount = data.potionCount;
        _potionCountText.text = "" + data.potionCount;

        Vector3 pos = new Vector3(data.position[0], data.position[1], data.position[2]);
        transform.position = pos;

        Vector3 campos = new Vector3(data.cameraPos[0], data.cameraPos[1], data.cameraPos[2]);
        _camera.transform.position = campos;
        LoadEnemies(data);
        PauseOrPlayGame(true);
    }

    private void LoadEnemies(PlayerData data)
    {
        List<GameObject> enemies = findEnemies();
        Debug.Log(enemies.Count);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<HealthScript>().enabled = data.healthEnabled[i];
            if (data.healthEnabled[i])
            {
                enemies[i].GetComponent<HealthScript>().setHealth(data.enemyHealths[i]);
                Vector3 pos = new Vector3(data.enemyPos[i][0], data.enemyPos[i][1], data.enemyPos[i][2]);
                enemies[i].transform.position = pos;
            }
            else
            {
                enemies[i].SetActive(false);
            }
        }
    }
    // In Game Menu 
    public void PauseOrPlayGame(bool comeback)
    {
        if (Input.GetButtonDown("Pause") || comeback)
        {
            if (inGameCanvas.activeSelf == true)
            {
                inGameCanvas.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                inGameCanvas.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

}
