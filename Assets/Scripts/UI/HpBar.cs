using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpBar : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    private int maxHp;

    public void InitHpBar(int maxHp, int currentHp) {
        this.maxHp = maxHp;
        hpBar.fillAmount = currentHp / (float)this.maxHp;
    }

    public void UpdateHpBar(int currentHp) {
        hpBar.fillAmount = currentHp / (float)maxHp;
    }

}
