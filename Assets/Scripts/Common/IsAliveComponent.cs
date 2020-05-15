using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAliveComponent : MonoBehaviour
{
    private Animator _currentActorAnimator;
    private Collider2D _currentActorDeathCollider;
    public bool isAlive = true;

    void Start()
    {
        _currentActorDeathCollider = transform.Find("Death Collider").GetComponent<Collider2D>();
        _currentActorAnimator = GetComponent<Animator>();
    }

    public void ActorDeathCondition()
    {
        isAlive = false;
        GetComponent<BoxCollider2D>().enabled = false;
        _currentActorDeathCollider.enabled = true;
        _currentActorAnimator.SetTrigger("Death");
    }
}
