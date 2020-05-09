using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private List<Sprite> _healthBarSrites = new List<Sprite>();
    [SerializeField] public int health;
    private Animator _currentActorAnimator;
    private IsAliveComponent _isAliveComponent;
    private ActorStatsController _playerStatsController;
    private ActorStatsController _enemyStatsController;
    private Image _healthBar;


    void Start()
    {
        _healthBar = GameObject.FindGameObjectWithTag("Canvas HP Bar").GetComponent<Image>();
        _healthBar.sprite = _healthBarSrites[4];
        _currentActorAnimator = GetComponent<Animator>();
        _isAliveComponent = GetComponent<IsAliveComponent>();
        _playerStatsController = GameObject.FindGameObjectWithTag("Player").GetComponent<ActorStatsController>();
    }

    public void TakeDamage(int damageTaken, string whoCausedDamage)
    {
        if(whoCausedDamage == "Enemy" || whoCausedDamage == "Trap")
        {
            if (health > 0)
            {
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
                _healthBar.sprite = _healthBarSrites[0];
                _isAliveComponent.ActorDeathCondition();
            }
        }
        else if(whoCausedDamage == "Player")
        {
            if (health > 0)
            {
                health -= damageTaken;
                _currentActorAnimator.SetTrigger("Hurt");
            }
            if (health <= 0)
            {
                _playerStatsController.GetExperienceForKill(); // gain XP for killing enemy
                _isAliveComponent.ActorDeathCondition();
            }
        }

    }

}
