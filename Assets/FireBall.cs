using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private Rigidbody2D _fireBallRigidBody;
    private Transform _playerTransform;
    private int _fireBallDamage = 1;
    private float _destroyTimer = 5.0f;

    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //_fireBallRigidBody.velocity = transform.right * _speed;
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player")
        {
            HealthComponent playerHealth = hitInfo.GetComponent<HealthComponent>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(_fireBallDamage, "Enemy");
            }
            Destroy(gameObject);
        }
        else if (hitInfo.tag == "Ground")
        {
            Destroy(gameObject);
        }
        
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _speed * Time.deltaTime);
        if (_destroyTimer > 0)
        {
            _destroyTimer -= Time.deltaTime;
        }
        else if(_destroyTimer <= 0)
        {
            Destroy(gameObject);
        }    
    }


}
