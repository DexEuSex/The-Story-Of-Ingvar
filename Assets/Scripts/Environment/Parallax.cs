using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float parallaxEffect;
    private float _length;
    private float _startpos;
    void Start()
    {
        _startpos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void Update()
    {
        float temp = mainCamera.transform.position.x * (1 - parallaxEffect);

        float distance = mainCamera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(_startpos + distance, transform.position.y, transform.position.z);

        if (temp > _startpos + _length)
        {
            _startpos += _length;
        }

        else if (temp < _startpos - _length)
        {
            _startpos -= _length;
        }
    }
}
