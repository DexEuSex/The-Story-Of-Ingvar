using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _healthBarSrites = new List<Sprite>();
    private Image _healthBar;
    void Start()
    {
        
        _healthBar = GetComponent<Image>();
    }

    public void SetFourHP()
    {
        _healthBar.sprite = _healthBarSrites[4];
    }

    public void SetThreeHP()
    {
        _healthBar.sprite = _healthBarSrites[3];
    }

    public void SetTwoHP()
    {
        _healthBar.sprite = _healthBarSrites[2];
    }

    public void SetOneHP()
    {
        _healthBar.sprite = _healthBarSrites[1];
    }

    public void SetZeroHP()
    {
        _healthBar.sprite = _healthBarSrites[0];
    }



}
