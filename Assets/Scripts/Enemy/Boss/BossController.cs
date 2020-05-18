using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private Transform _lightImitation;
    [SerializeField] private AudioSource _bossAudioSource;
    [SerializeField] private AudioClip _deathSFX;
    [SerializeField] private float _chasingSpeed;
    [SerializeField] private GameObject _playerCanvas;
    [SerializeField] private BossFightTrigger _bossFightTrigger;
    [SerializeField] private GameObject _woundedCollider; // заменяет собой основной коллайдер во время анимации ранения, чтобы этот персонаж не летал в воздухе

    private Rigidbody2D _bossRigidBody;
    private HealthComponent _bossHealthComponent;
    private IsAliveComponent _bossIsAliveComponent;
    private Transform _playerTransform;
    private IsAliveComponent _playerIsAliveComponent;
    private SpriteRenderer _bossSprite;
    private Animator _bossAnimator;
    private Collider2D _bossCollider;
    private Collider2D _finalBossDialogueTrigger; // он будет активирован после того как игрок отнимет у босса достаточное количество очков жизни
    private GameObject _playerEnterSensor; // будет использовано для запрета боссу бить мечом если он ранен
    private GameObject _bossLastDialogueObj; // будет использовано для проверки, закончился ли последний диалог. Если да, стартует концовка уровня

    // переменные, не позволяющие делать одни и те же действия в Update если они уже были сделаны однажды
    private bool _isWoundedSfxPlayed = false;
    private bool _isDeathSfxPlayed = false;
    private bool _isLevelEndStarted = false; 
    private bool _isCollisionAndCanvasActive = true;

    public bool isWoundedAnimationPlayedOnce = false;// переменная, которая регисриует включение анимации ранения
    public bool isInWoundedAnimation;
    


    void Start()
    {
        _playerEnterSensor = GameObject.FindGameObjectWithTag("Boss Player Enter Sensor").gameObject;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _bossLastDialogueObj = GameObject.Find("Boss Dialogue 2").gameObject;

        _bossCollider = GetComponent<Collider2D>();
        _bossRigidBody = GetComponent<Rigidbody2D>();
        _bossHealthComponent = GetComponent<HealthComponent>();
        _bossIsAliveComponent = GetComponent<IsAliveComponent>();
        _bossAnimator = GetComponent<Animator>();
        _bossSprite = GetComponent<SpriteRenderer>();
        _finalBossDialogueTrigger = _bossLastDialogueObj.GetComponent<Collider2D>();

        _playerIsAliveComponent = _playerTransform.GetComponent<IsAliveComponent>();
        _bossHealthComponent.isItBoss = true;
    }

    void Update()
    {
        if (_playerIsAliveComponent.isAlive && _playerIsAliveComponent.isAlive && !_bossHealthComponent.isLightlyWounded && !_bossHealthComponent.isSeriouslyWounded && !DialogManager.isDialogOpened)
        {
            ChasePlayer();
        }

        else if (!_playerIsAliveComponent.isAlive)
        {
            EnableIDLEAnimation();
        }
        
        // так как это условие должно выполниться только один раз в середине боя, 
        // была создана специальная переменная, не позволяющая включить её снова
        if (_bossHealthComponent.isLightlyWounded && !isWoundedAnimationPlayedOnce) 
        {
            if(!_isWoundedSfxPlayed)
            {
                if (!AudioManager.IsPlayingSfx(AudioManager._audioManagerInner.bossSFXAudioSource))
                {
                    AudioManager.PlayBossWoundedSfx(AudioManager._audioManagerInner.bossSFXAudioSource);
                }
                _isWoundedSfxPlayed = true;
            }

            StartCoroutine(EnableWoundedAnimation(false));
        }

        if(_bossHealthComponent.isSeriouslyWounded)
        {
            StartCoroutine(EnableWoundedAnimation(true));
            StartCoroutine("EnableLastDialogue");
        }

        if (!_bossLastDialogueObj.activeSelf) // если последний диалог закончился, уровень заканчивается
        {
            if(!_isLevelEndStarted)
            {
                _bossFightTrigger.EndBossFight();
                _isLevelEndStarted = true;
            }
                
            if(_isCollisionAndCanvasActive)
            {
                _woundedCollider.SetActive(false);
                _bossCollider.enabled = false;
                _bossRigidBody.gravityScale = 0;
                _playerCanvas.SetActive(false);
                _isCollisionAndCanvasActive = false;
            }
            

            if (!_isDeathSfxPlayed)
            {
                if (!AudioManager.IsPlayingSfx(_bossAudioSource))
                {
                    _isDeathSfxPlayed = true;
                    _bossAudioSource.PlayOneShot(_deathSFX);
                }
            }


            transform.position = Vector2.MoveTowards(transform.position, _lightImitation.position, _chasingSpeed * Time.deltaTime);

            StartCoroutine("EnableLoopedHurtAnimation");

            if(transform.position.Equals(_lightImitation.position))
            {
                _lightImitation.gameObject.SetActive(true);
                _lightImitation.localScale = new Vector3(_lightImitation.localScale.x + 0.01f, _lightImitation.localScale.y + 0.01f);
            }

        }
        
    }

    
    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _chasingSpeed * Time.deltaTime);

        EnableRunAnimaton();

        CheckFlipping();
    }

    void EnableRunAnimaton()
    {
        _bossAnimator.SetInteger("AnimState", 2);
        _bossAnimator.speed = 0.8f;
    }

    void CheckFlipping()
    {
        if (transform.position.x > _playerTransform.position.x)
        {
            _bossSprite.flipX = false;
        }
        else if (transform.position.x < _playerTransform.position.x)
        {
            _bossSprite.flipX = true;
        }
    }

    void Flip() 
    {
        transform.Rotate(0f, 180f, 0f);
    }

    void EnableIDLEAnimation()
    {
        _bossAnimator.SetInteger("AnimState", 0);
    }

    IEnumerator EnableWoundedAnimation(bool isLast)
    {
        if(!isLast)
        {
            DisableAttackLogic();

            _bossHealthComponent.BecomeInvulnerable(); // дабы игрок не мог убить босса пока тот отдыхает, ему передаётся неуязвимость

            isInWoundedAnimation = true;

            GetComponent<Collider2D>().enabled = false;

            _woundedCollider.SetActive(true);

            _bossAnimator.SetInteger("AnimState", 4);

            yield return new WaitForSeconds(2.6f);

            _bossHealthComponent.isLightlyWounded = false;

            _woundedCollider.SetActive(false);

            GetComponent<Collider2D>().enabled = true;

            isInWoundedAnimation = false;

            isWoundedAnimationPlayedOnce = true; // тот самый счётчик

            _bossHealthComponent.BecomeVulnerable();

            EnableAttackLogic();
        }

        else if(isLast)
        {
            _bossHealthComponent.BecomeInvulnerable();

            DisableAttackLogic();

            isInWoundedAnimation = true;

            GetComponent<Collider2D>().enabled = false;

            _woundedCollider.SetActive(true);

            _bossAnimator.SetInteger("AnimState", 4);
        }
    }

    private void DisableAttackLogic()
    {
        _playerEnterSensor.SetActive(false);
    }

    private void EnableAttackLogic()
    {
        _playerEnterSensor.SetActive(true);
    }

    IEnumerator EnableLoopedHurtAnimation()
    {
        _bossAnimator.SetTrigger("Hurt");
        yield return new WaitForSeconds(1.00f);
    }

    IEnumerator EnableLastDialogue()
    {
        yield return new WaitForSeconds(1.30f);
        _finalBossDialogueTrigger.enabled = true;
    }


   


}

