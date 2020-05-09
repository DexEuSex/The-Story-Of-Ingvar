using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorStatsController : MonoBehaviour
{
    public int actorLevel = 0;
    public int actorDamage = 1;
    private HealthComponent _healthComponent;
    private UIValuesController _uiValuesController;


    void Start()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _uiValuesController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIValuesController>();
    }

    public void LevelUp()
    {
        _healthComponent.health += 1;
        actorDamage += 1;
        actorLevel += 1;
    }

    public void GetExperienceForKill()
    {
        _uiValuesController.XPBarValueUpdate(10); // 10 exp points for each killing by far
    }

}
