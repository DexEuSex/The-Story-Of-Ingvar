using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] private GameObject _bloodParticle;
    [SerializeField] private GameObject _bloodSprite;

    private Animator _currentActorAnimator;
    private Transform _currentTransform;
    private IsAliveComponent _isAliveComponent;
    private ActorStatsController _playerStatsController;
    private ActorStatsController _enemyStatsController;
    private HealthBarController _healthBarController;
    private bool _isSticked;

    public bool isLightlyWounded = false;
    public bool isSeriouslyWounded = false;
    public bool isInvulnerable = false;
    public bool isItBoss = false;

    void Start()
    {
        _healthBarController = GameObject.Find("HP Bar").GetComponent<HealthBarController>();
        _currentActorAnimator = GetComponent<Animator>();
        _isAliveComponent = GetComponent<IsAliveComponent>();
        _currentTransform = GetComponent<Transform>();
        _playerStatsController = GetComponent<ActorStatsController>();
    }

    public void TakeDamage(int damageTaken, string whoCausedDamage)
    {
        if(isInvulnerable)
        {
            return;
        }

        if(whoCausedDamage == "Enemy" || whoCausedDamage == "Trap")
        {
            if (health > 0)
            {
                StartCoroutine("BloodDripping");
                health -= damageTaken;
                AudioManager.PlayHurtSfx(AudioManager._audioManagerInner.hurtAudioSource);
                _currentActorAnimator.SetTrigger("Hurt");
                if (health == 3)
                    _healthBarController.SetThreeHP();
                else if (health == 2)
                    _healthBarController.SetTwoHP();
                else if (health == 1)
                    _healthBarController.SetOneHP();
            }
            if (health <= 0)
            {
                _bloodParticle.SetActive(false);
                _healthBarController.SetZeroHP();
                _isAliveComponent.ActorDeathCondition();
            }
        }

        else if(whoCausedDamage == "Player")
        {
            if(isItBoss)
            {
                if (health <= 5)
                {
                    isSeriouslyWounded = true;
                }

                if (health <= 10)
                {
                    isLightlyWounded = true;
                }

                if (health > 0)
                {
                    AudioManager.PlayBossHurtSfx(AudioManager._audioManagerInner.bossSFXAudioSource);
                    StartCoroutine("BloodDripping");
                    health -= damageTaken;
                    _currentActorAnimator.SetTrigger("Hurt");
                }
            }
            if(!isItBoss)
            {
                if (health > 0)
                {
                    AudioManager.PlayEnemyHurtSfx(AudioManager._audioManagerInner.enemyHurtAudioSource);
                    StartCoroutine("BloodDripping");
                    health -= damageTaken;
                    _currentActorAnimator.SetTrigger("Hurt");
                }

                if (health <= 0)
                {
                    AudioManager.PlayEnemyDeathSfx(AudioManager._audioManagerInner.enemyHurtAudioSource);
                    _bloodParticle.SetActive(false);
                    _playerStatsController.GetExperienceForKill(); // gain XP for killing enemy
                    _isAliveComponent.ActorDeathCondition();
                    StartCoroutine("EnableBloodOnGround");
                }
            }
        }
    }

    IEnumerator BloodDripping()
    {
        _isSticked = true;
        _bloodParticle.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _bloodParticle.SetActive(false);
        _isSticked = false;
    }

    void Update()
    {
        if(_isSticked)
        {
            _bloodParticle.transform.position = transform.position;
        }
    }

    IEnumerator EnableBloodOnGround()
    {
        yield return new WaitForSeconds(0.5f);
        _bloodSprite.SetActive(true);
        _bloodSprite.transform.position = new Vector3
            (
            _currentTransform.transform.position.x,
            _currentTransform.transform.position.y - 13.0f,
            _currentTransform.transform.position.z
            );
    }

    public void BecomeInvulnerable()
    {
        isInvulnerable = true;
    }
    public void BecomeVulnerable()
    {
        isInvulnerable = false;
    }



}
