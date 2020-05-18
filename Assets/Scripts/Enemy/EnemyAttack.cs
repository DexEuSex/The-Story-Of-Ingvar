using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private bool _inAttack = false;
    [SerializeField] private float _timeCounter;
    [SerializeField] private float timeBetweenAttack;
    [SerializeField] private Transform attackPose;
    [SerializeField] private Transform attackPoseFlipX;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask whoIsEnemyToThisActor;
    

    private ActorStatsController _actorStatsController;
    private Animator _enemyCombatAnimator;
    private IsAliveComponent _isAliveComponent;

    void Start()
    {
        _isAliveComponent = GetComponent<IsAliveComponent>();
        _enemyCombatAnimator = GetComponent<Animator>();
        _actorStatsController = GetComponent<ActorStatsController>();
    }
    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPose.position, attackRange);
        Gizmos.DrawWireSphere(attackPoseFlipX.position, attackRange);
    }

    public void EnemyAttackLogic()
    {
        if (_timeCounter <= 0 && !_inAttack)
        {
            if (GetComponent<SpriteRenderer>().flipX)
                StartCoroutine("DealDamageToPlayerFlipX");
            else
                StartCoroutine("DealDamageToPlayer");
        }
        else
        {
            _timeCounter -= Time.deltaTime;
        }

    }

    IEnumerator DealDamageToPlayer()
    {
        _inAttack = true;
        _enemyCombatAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        Collider2D playerToDamage = Physics2D.OverlapCircle(attackPose.position, attackRange, whoIsEnemyToThisActor);
        if(playerToDamage != null)
        {
            playerToDamage.GetComponent<HealthComponent>().TakeDamage(_actorStatsController.actorDamage, "Enemy");
        }
        _timeCounter = timeBetweenAttack;
        _inAttack = false;
        StartCoroutine("StartCombatIdle");
    }

    IEnumerator DealDamageToPlayerFlipX()
    {
        _inAttack = true;
        _enemyCombatAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        Collider2D playerToDamage = Physics2D.OverlapCircle(attackPoseFlipX.position, attackRange, whoIsEnemyToThisActor);
        if (playerToDamage != null)
        {
            playerToDamage.GetComponent<HealthComponent>().TakeDamage(_actorStatsController.actorDamage, "Enemy");
        }
        _timeCounter = timeBetweenAttack;
        _inAttack = false;
        StartCoroutine("StartCombatIdle");
    }

    IEnumerator StartCombatIdle()
    {
        yield return new WaitForSeconds(0.5f);
        _enemyCombatAnimator.SetInteger("AnimState", 1);
        yield return new WaitForSeconds(5.0f);
        _enemyCombatAnimator.SetInteger("AnimState", 0);
    }

    void Update()
    {
        
    }

    public bool IsInAttack()
    {
        return _inAttack;
    }

}
