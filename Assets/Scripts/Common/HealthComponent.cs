using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private List<Sprite> _healthBarSrites = new List<Sprite>();
    [SerializeField] public int health;
    [SerializeField] private GameObject _bloodParticle;
    [SerializeField] private GameObject _bloodSprite;
    private Animator _currentActorAnimator;
    private Transform _currentTransform;
    private IsAliveComponent _isAliveComponent;
    private ActorStatsController _playerStatsController;
    private ActorStatsController _enemyStatsController;
    private Image _healthBar;
    private bool _isSticked;

    void Start()
    {
        _healthBar = GameObject.FindGameObjectWithTag("Canvas HP Bar").GetComponent<Image>();
        _healthBar.sprite = _healthBarSrites[4];
        _currentActorAnimator = GetComponent<Animator>();
        _isAliveComponent = GetComponent<IsAliveComponent>();
        _currentTransform = GetComponent<Transform>();
        _playerStatsController = GameObject.FindGameObjectWithTag("Player").GetComponent<ActorStatsController>();
    }

    public void TakeDamage(int damageTaken, string whoCausedDamage)
    {
        if(whoCausedDamage == "Enemy" || whoCausedDamage == "Trap")
        {
            if (health > 0)
            {
                StartCoroutine("BloodDripping");
                health -= damageTaken;
                _currentActorAnimator.SetTrigger("Hurt");
                if (health == 3)
                    _healthBar.sprite = _healthBarSrites[3];
                else if (health == 2)
                    _healthBar.sprite = _healthBarSrites[2];
                else if (health == 1)
                    _healthBar.sprite = _healthBarSrites[1];
            }
            if (health <= 0)
            {
                _bloodParticle.SetActive(false);
                _healthBar.sprite = _healthBarSrites[0];
                _isAliveComponent.ActorDeathCondition();
            }
        }
        else if(whoCausedDamage == "Player")
        {
            if (health > 0)
            {
                StartCoroutine("BloodDripping");
                health -= damageTaken;
                _currentActorAnimator.SetTrigger("Hurt");
            }
            if (health <= 0)
            {
                _bloodParticle.SetActive(false);
                _playerStatsController.GetExperienceForKill(); // gain XP for killing enemy
                _isAliveComponent.ActorDeathCondition();
                StartCoroutine("EnableBloodOnGround");
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
            _bloodParticle.transform.position = _currentTransform.transform.position;
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

}
