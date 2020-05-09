using UnityEngine;
using System.Collections;

public class GroundSensor : MonoBehaviour 
{
    private int _colCount = 0;
    private static float _disableTimer;
    public bool isGround = false;
    private Vector3 _startPos;
    private Transform _parentPlayerActor;
    private HealthComponent _parentHealthComponent;
    private Animator _parentPlayerAnimator;

    void Start()
    {
        _parentPlayerAnimator = GetComponentInParent<Animator>();
        _parentHealthComponent = GetComponentInParent<HealthComponent>();
        _parentPlayerActor = transform.parent.GetComponent<Transform>();
        _startPos = _parentPlayerActor.transform.position;
    }

    void Update()
    {
        _disableTimer -= Time.deltaTime;
    }

    private void OnEnable()
    {
        _colCount = 0;
    }

    public bool State()
    {
        if (_disableTimer > 0)
        {
            return false;
        }
        return _colCount > 0;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            isGround = true;
        }

        else if (collider.tag == "Dead Zone")
        {
            _parentPlayerActor.transform.position = _startPos;
        }
        
        else if(collider.tag == "Trap")
        {
            isGround = true;
            _parentHealthComponent.TakeDamage(1, "Trap");
        }


    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            isGround = false;
        }
    }


    public void Disable(float duration)
    {
        _disableTimer = duration;
    }


}
