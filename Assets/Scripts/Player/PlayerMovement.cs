using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField] private float _currTime = 1f;
    private Animator _playerAnimator;
    private GroundSensor _playerGroundSensor;
    private IsAliveComponent _isAliveComponent;
    private bool _jumpOffEnable = false;
    private int _enemyObjectLayer;
    private int _playerObjectLayer;
    private int _specialPlatformLayer;
    private int _jumpCounter = 0;
    Rigidbody2D playerBody2D;
    

    void Start()
    {
        _isAliveComponent = GetComponent<IsAliveComponent>();
        _playerAnimator = GetComponent<Animator>();
        playerBody2D = GetComponent<Rigidbody2D>();
        _playerGroundSensor = transform.Find("Ground Sensor").GetComponent<GroundSensor>();
        _playerObjectLayer = LayerMask.NameToLayer("Player");
        _enemyObjectLayer = LayerMask.NameToLayer("Enemy");
        _specialPlatformLayer = LayerMask.NameToLayer("Special Platform");
    }

    
    void Update()
    {
        
        Physics2D.IgnoreLayerCollision(_playerObjectLayer, _enemyObjectLayer, true);

        if (_playerGroundSensor.isGround)
        {
            _playerAnimator.SetBool("Grounded", true);
            _jumpCounter = 0;
        }

        if(Input.GetKeyDown(KeyCode.W) && _playerGroundSensor.isGround && _isAliveComponent.isAlive && !EscMenuController.isMenuActive)
        {
            Jump();
            _jumpCounter++;

        }

        else if (Input.GetKeyDown(KeyCode.W) && !_playerGroundSensor.isGround && _isAliveComponent.isAlive && !EscMenuController.isMenuActive)
        {
            if (_jumpCounter < 2)
            {
                Jump();
            }
            _jumpCounter = 2;
        }
        if(_isAliveComponent.isAlive && !EscMenuController.isMenuActive)
        {
            BasicMovement();
            JumpThroughPlafrom();
        }

        if (Input.GetKey(KeyCode.S) && _isAliveComponent.isAlive && !EscMenuController.isMenuActive)
        {
            StartCoroutine("JumpOffPlatform");
        }

    }
    
    // All movement logic is below 
    private void BasicMovement()
    {
        float move = Input.GetAxis("Horizontal");

        if (Mathf.Abs(move) > Mathf.Epsilon && _playerGroundSensor.isGround)
        {
            _playerAnimator.SetInteger("AnimState", 2);
        }
        else
        {
            _playerAnimator.SetInteger("AnimState", 0);
        }

        playerBody2D.velocity = new Vector2(move * _moveSpeed, playerBody2D.velocity.y);

        // Playing walking SFX
        if (playerBody2D.velocity.x > 0 && _playerGroundSensor.isGround)
        {
            if (!AudioManager.IsPlayingSfx(AudioManager._audioManagerInner.movementAudioSource))
            {
                AudioManager.PlayWalkingSfx();
            }
        }
        else if (playerBody2D.velocity.x < 0 && _playerGroundSensor.isGround)
        {
            if (!AudioManager.IsPlayingSfx(AudioManager._audioManagerInner.movementAudioSource))
            {
                AudioManager.PlayWalkingSfx();
            }
        }
        else AudioManager.StopPlayingSfx(AudioManager._audioManagerInner.movementAudioSource);
        // ----

        if (move > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (move < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    private void Jump()
    {
        _playerAnimator.SetTrigger("Jump");
        AudioManager.PlayJumpSfx(AudioManager._audioManagerInner.jumpAudioSource);
        _playerGroundSensor.isGround = false;
        _playerAnimator.SetBool("Grounded", _playerGroundSensor.isGround);
        playerBody2D.velocity = new Vector2(playerBody2D.velocity.x, _jumpForce);

    }
    

    void JumpThroughPlafrom()
    {
        if (playerBody2D.velocity.y > 0)
        {
            StartCoroutine("JumpOnThePlatform");
            //Physics2D.IgnoreLayerCollision(_playerObjectLayer, _specialPlatformLayer, true);
        }
        //else
        //{
        //    Physics2D.IgnoreLayerCollision(_playerObjectLayer, _specialPlatformLayer, false);
        //}
    }

    IEnumerator JumpOffPlatform()
    {
        _jumpOffEnable = true;
        Physics2D.IgnoreLayerCollision(_playerObjectLayer, _specialPlatformLayer, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(_playerObjectLayer, _specialPlatformLayer, false);
        _jumpOffEnable = false;
    }

    IEnumerator JumpOnThePlatform()
    {
        _jumpOffEnable = true;
        Physics2D.IgnoreLayerCollision(_playerObjectLayer, _specialPlatformLayer, true);
        yield return new WaitForSeconds(0.8f);
        Physics2D.IgnoreLayerCollision(_playerObjectLayer, _specialPlatformLayer, false);
        _jumpOffEnable = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.tag == "enemy")
        {
            Physics2D.IgnoreLayerCollision(_playerObjectLayer, _enemyObjectLayer, true);
        }
        else if (collider.tag == "deadEnemy")
        {
            Physics2D.IgnoreLayerCollision(_playerObjectLayer, _enemyObjectLayer, true);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "enemy")
        {
            Physics2D.IgnoreLayerCollision(_playerObjectLayer, _enemyObjectLayer, true);
        }
        else if (collider.tag == "deadEnemy")
        {
            Physics2D.IgnoreLayerCollision(_playerObjectLayer, _enemyObjectLayer, true);
        }
    }

    public void PlayWalkingSfx()
    {
        AudioManager.PlaySfx(SfxType.Walking, AudioManager._audioManagerInner.movementAudioSource);
    }

    public static void DeactivateControls()
    {
        EscMenuController.isMenuActive = true; // Если эта переменная true, то элементы управления не работают
    }

    public static void ActivateControls()
    {
        EscMenuController.isMenuActive = false;
    }

    private void Flip()
    {
        //if(GetComponent<SpriteRenderer>().flipX)
        //{
        //    GetComponent<SpriteRenderer>().flipX = false;
        //}
        //else if (!GetComponent<SpriteRenderer>().flipX)
        //{
        //    GetComponent<SpriteRenderer>().flipX = true;
        //}
        transform.Rotate(0f, 180f, 0f);
    }

}
