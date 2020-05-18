using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField] public float _timer;
    [SerializeField] public float _timeBetweenCasting;

    private HealthComponent _bossHealthComponent;
    private SpriteRenderer _bossSprite;
    private IsAliveComponent _playerIsAliveComponent;
    private IsAliveComponent _bossIsAliveComponent;
    private BossController _bossController;
    private Animator _bossAnimator;

    public GameObject fireBalltPrefab;
    public Transform firePoint;
    public Transform firePointFlipX;

    private void Start()
    {
        firePoint.Rotate(0f, 180f, 0f); // Для того, чтобы босс стрелял в нужную сторону, так как трансформ всегда изначально повёрнут вправо. Эта строка разворачивает его влево
        // Эта переменная не разрешит боссу стрелять если он валяется раненный
        _bossController = GameObject.Find("Boss").GetComponent<BossController>();
        _playerIsAliveComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<IsAliveComponent>();
        _bossIsAliveComponent = GameObject.FindGameObjectWithTag("Boss").GetComponent<IsAliveComponent>();
        _bossAnimator = GameObject.Find("Boss").GetComponent<Animator>();
        _bossHealthComponent = GetComponent<HealthComponent>();
        _bossSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(_bossHealthComponent.health <= 10 && _playerIsAliveComponent.isAlive && _bossIsAliveComponent.isAlive && !_bossController.isInWoundedAnimation && _bossController.isWoundedAnimationPlayedOnce)
        {
            if(_timer <= 0)
            {
                CastFireball();
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
    }

    void CastFireball()
    {
        if (!_bossSprite.flipX)
        {
            _bossAnimator.SetTrigger("Attack");
            Instantiate(fireBalltPrefab, firePoint.position, firePoint.rotation);
            _timer = _timeBetweenCasting;
        }
        else if(_bossSprite.flipX)
        {
            _bossAnimator.SetTrigger("Attack");
            Instantiate(fireBalltPrefab, firePointFlipX.position, firePointFlipX.rotation);
            _timer = _timeBetweenCasting;
        }
        
    }

}
