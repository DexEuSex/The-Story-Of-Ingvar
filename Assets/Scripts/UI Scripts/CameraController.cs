
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private float _cameraDamping = 1.5f;
    private Vector2 _cameraOffset = new Vector2(2f, 1f);
    private bool _isLeft; // checking if the player is looking to the left
    private Transform _playerTransform;
    private int _lastX;

    [SerializeField] private float _leftLimit;
    [SerializeField] private float _rightLimit;
    [SerializeField] private float _bottomLimit;
    [SerializeField] private float _upperLimit;

    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _cameraOffset = new Vector2(Mathf.Abs(_cameraOffset.x), _cameraOffset.y);
        FindPlayer(_isLeft);
    }
    

    void Update()
    {
        if(_playerTransform)
        {
            int currentX = Mathf.RoundToInt(_playerTransform.position.x);
            if (currentX > _lastX)
                _isLeft = false;
            else if (currentX < _lastX)
                _isLeft = true;
            _lastX = Mathf.RoundToInt(_playerTransform.position.x);
            Vector3 target;

            if(_isLeft)
            {
                target = new Vector3(_playerTransform.position.x - _cameraOffset.x, _playerTransform.position.y + _cameraOffset.y, transform.position.z);
            }
            else
            {
                target = new Vector3(_playerTransform.position.x + _cameraOffset.x, _playerTransform.position.y + _cameraOffset.y, transform.position.z);
            }

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, _cameraDamping * Time.deltaTime);
            transform.position = currentPosition;
        }

        transform.position = new Vector3
            (
            Mathf.Clamp(transform.position.x, _leftLimit, _rightLimit),
            Mathf.Clamp(transform.position.y, _bottomLimit, _upperLimit),
            transform.position.z
            );

    }

    void FindPlayer(bool playerIsLeft)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _lastX = Mathf.RoundToInt(_playerTransform.position.x);
        if(playerIsLeft)
        {
            transform.position = new Vector3(_playerTransform.position.x - _cameraOffset.x, _playerTransform.position.y - _cameraOffset.y, transform.position.z);
        }
        else 
        {
            transform.position = new Vector3(_playerTransform.position.x + _cameraOffset.x, _playerTransform.position.y + _cameraOffset.y, transform.position.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(_leftLimit, _upperLimit), new Vector2(_rightLimit, _upperLimit));
        Gizmos.DrawLine(new Vector2(_leftLimit, _bottomLimit), new Vector2(_rightLimit, _bottomLimit));
        Gizmos.DrawLine(new Vector2(_leftLimit, _upperLimit), new Vector2(_leftLimit, _bottomLimit));
        Gizmos.DrawLine(new Vector2(_rightLimit, _upperLimit), new Vector2(_rightLimit, _bottomLimit));
    }

}