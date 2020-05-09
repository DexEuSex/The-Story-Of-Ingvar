using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private ActorStatsController _actorStatsController;
    private Animator _enemyCombatAnimator;
    private IsAliveComponent _isAliveComponent;
    public bool _inAttack = false;
    public float _timeCounter;
    public float timeBetweenAttack;
    public Transform attackPose;
    public Transform attackPoseFlipX;
    public float attackRange;
    public LayerMask whoIsEnemyToThisActor;

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
        if (_timeCounter <= 0 && _inAttack == false)
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
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPose.position, attackRange, whoIsEnemyToThisActor);
        for (int i = 0; i < playerToDamage.Length; i++)
        {
            playerToDamage[i].GetComponent<HealthComponent>().TakeDamage(_actorStatsController.actorDamage, "Enemy");
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
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPoseFlipX.position, attackRange, whoIsEnemyToThisActor);
        for (int i = 0; i < playerToDamage.Length; i++)
        {
            playerToDamage[i].GetComponent<HealthComponent>().TakeDamage(_actorStatsController.actorDamage, "Enemy");
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

}
