using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIValuesController : MonoBehaviour
{
    private Slider _xpBar;
    private TextMeshProUGUI _lvlDisplay;
    private ActorStatsController _actorStatsController;

    void Start()
    {
        _actorStatsController = GameObject.Find("Player").GetComponent<ActorStatsController>();
        _xpBar = GameObject.FindGameObjectWithTag("XP Bar Slider").GetComponent<Slider>();
        _lvlDisplay = GameObject.Find("Level Display").GetComponent<TextMeshProUGUI>();
        _lvlDisplay.SetText($"Level {_actorStatsController.actorLevel}");
    }


    public void XPBarValueUpdate(int valueToBeAdded)
    {
        if (_xpBar.value < 100)
        {
            _xpBar.value += valueToBeAdded;
        }
        else if (_xpBar.value >= 100)
        {
            _xpBar.value = 0;
            _actorStatsController.LevelUp();
            LevelDisplayUpdate(_actorStatsController.actorLevel);
        }
    }

    public void LevelDisplayUpdate(int levelToDisplay)
    {
        _lvlDisplay.text = $"Level {levelToDisplay}";
    }
}
