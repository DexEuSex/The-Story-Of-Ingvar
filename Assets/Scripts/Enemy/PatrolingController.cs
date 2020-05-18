using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class PatrolingController : MonoBehaviour
{

    [SerializeField] private float _patrolSpeed;
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private int _patrolDistance;
    [SerializeField] private Transform _pointToReturn;

    private Transform _playerTransform;
    private SpriteRenderer _npcSprite;
    private Animator _npcAnimator;
    private IsAliveComponent _npcIsAlive;

    private bool _movingRight;
    private bool isPeaceCondition = false;
    private bool isChasingCondition = false;
    private bool isReturningToThePatrolPoint = false;

    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _npcAnimator = GetComponent<Animator>();
        _npcIsAlive = GetComponent<IsAliveComponent>();
        _npcSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, _pointToReturn.position) < _patrolDistance && !isChasingCondition)
        {
            isPeaceCondition = true;
        }

        if(Vector2.Distance(transform.position, _playerTransform.position) < _stoppingDistance)
        {
            isChasingCondition = true;
            isPeaceCondition = false;
            isReturningToThePatrolPoint = false;
        }

        if(Vector2.Distance(transform.position, _playerTransform.position) > _stoppingDistance)
        {
            isReturningToThePatrolPoint = true;
            isChasingCondition = false;
        }

        if(isPeaceCondition && _npcIsAlive.isAlive)
        {
            PeaceCondition();
        }

        else if(isChasingCondition && _npcIsAlive.isAlive)
        {
            ChasingCondition();
        }

        else if(isReturningToThePatrolPoint && _npcIsAlive.isAlive)
        {
            ReturningToThePatrolPoint();
        }


    }

    void PeaceCondition()
    {
        if(transform.position.x > _pointToReturn.position.x + _patrolDistance)
        {
            _movingRight = false;
            _npcSprite.flipX = false;
        }
        else if(transform.position.x < _pointToReturn.position.x - _patrolDistance)
        {
            _npcSprite.flipX = true;
            _movingRight = true;
        }

        if(_movingRight)
        {
            _npcAnimator.SetInteger("AnimState", 2);
            transform.position = new Vector2(transform.position.x + _patrolSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            _npcAnimator.SetInteger("AnimState", 2);
            transform.position = new Vector2(transform.position.x - _patrolSpeed * Time.deltaTime, transform.position.y);
        }

    }

    void ChasingCondition()
    {
        _npcAnimator.SetInteger("AnimState", 2);
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _patrolSpeed * Time.deltaTime);
    }

    void ReturningToThePatrolPoint()
    {
        _npcAnimator.SetInteger("AnimState", 2);
        transform.position = Vector2.MoveTowards(transform.position, _pointToReturn.position, _patrolSpeed * Time.deltaTime);
    }


}
