using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsCounter : MonoBehaviour
{
    [SerializeField] private Image actionsCount, bg, indicator;

    private const float _2Actions = 0.219f;
    private const float _3Actions = 0.317f;
    private const float _4Actions = 0.414f;
    private const float _5Actions = 0.512f;

    private void OnEnable() {
        int _actionsCount = Player.Instance.MaxActionCount;

        if(_actionsCount == 2) {
            actionsCount.fillAmount = _2Actions;
            bg.fillAmount = .2f;
        }
        else if(_actionsCount == 3) {
            actionsCount.fillAmount = _3Actions;
            bg.fillAmount = .3f;
        }
        else if(_actionsCount == 4) {
            actionsCount.fillAmount = _4Actions;
            bg.fillAmount = .4f;
        } 
        else if (_actionsCount == 5) {
            actionsCount.fillAmount = _5Actions;
            bg.fillAmount = .5f;
        } else {
            actionsCount.fillAmount = _2Actions;
            bg.fillAmount = .2f;
        }
    }

    public void UpdateActionsIndicator() {
        int _actionsCount = Player.Instance.ActionCount;
        indicator.fillAmount = (float)_actionsCount / 10;
    }
}
