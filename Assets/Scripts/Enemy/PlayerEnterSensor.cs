using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterSensor : MonoBehaviour
{
    private EnemyAttack _enemyAttack;
    private IsAliveComponent _isAliveComponent;
    void Start()
    {
        _enemyAttack = GetComponentInParent<EnemyAttack>();
        _isAliveComponent = GetComponentInParent<IsAliveComponent>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && _isAliveComponent.isAlive)
        {
            _enemyAttack.EnemyAttackLogic();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player" && _isAliveComponent.isAlive)
        {
            _enemyAttack.EnemyAttackLogic();
        }
    }



}
