using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private IsAliveComponent _IsAliveComponent;
    private EscMenuController _escMenuController;
    private ActorStatsController _actorStatsController;
    private Animator _playerCombatAnimator;
    private bool _inAttack = false;
    [SerializeField] public float _timeCounter;
    [SerializeField] public float timeBetweenAttack;
    [SerializeField] public Transform attackPose;
    [SerializeField] public Transform attackPoseFlipX;
    [SerializeField] public float attackRange;
    [SerializeField] public LayerMask whoIsEnemyToThisActor;

    void Start()
    {
        _escMenuController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<EscMenuController>();
        _playerCombatAnimator = GetComponent<Animator>();
        _IsAliveComponent = GetComponent<IsAliveComponent>();
        _actorStatsController = GetComponent<ActorStatsController>();
    }

    void Update()
    {
        if(!_escMenuController.isMenuActive)
        {
            AttackLogic();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPose.position, attackRange);
        Gizmos.DrawWireSphere(attackPoseFlipX.position, attackRange);
    }

    void AttackLogic()
    {
        if (_timeCounter <= 0 && _IsAliveComponent.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_inAttack)
            {
                if(GetComponent<SpriteRenderer>().flipX)
                    StartCoroutine("DealDamageToEnemyFlipX");
                else
                    StartCoroutine("DealDamageToEnemy");
            }
        }
        else
        {
            _timeCounter -= Time.deltaTime;
        }
    }

    IEnumerator DealDamageToEnemy()
    {
        _inAttack = true;
        _playerCombatAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPose.position, attackRange, whoIsEnemyToThisActor);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<HealthComponent>().TakeDamage(_actorStatsController.actorDamage, "Player");
        }
        _timeCounter = timeBetweenAttack;
        _inAttack = false;
        StartCoroutine("StartCombatIdle");
    }

    IEnumerator DealDamageToEnemyFlipX()
    {
        _inAttack = true;
        _playerCombatAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoseFlipX.position, attackRange, whoIsEnemyToThisActor);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<HealthComponent>().TakeDamage(_actorStatsController.actorDamage, "Player");
        }
        _timeCounter = timeBetweenAttack;
        _inAttack = false;
        StartCoroutine("StartCombatIdle");
    }

    IEnumerator StartCombatIdle()
    {
        yield return new WaitForSeconds(0.5f);
        _playerCombatAnimator.SetInteger("AnimState", 1);
        yield return new WaitForSeconds(5.0f);
        _playerCombatAnimator.SetInteger("AnimState", 0);
    }

}
