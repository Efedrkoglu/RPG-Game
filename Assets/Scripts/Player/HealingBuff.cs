using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBuff : Buff
{
    private int healingAmount;

    public HealingBuff(int duration, int healingAmount) : base(duration, 1) {
        this.healingAmount = healingAmount;
        ApplyBuff();
    }

    protected override void ApplyBuff() {
        Player.Instance.CurrentHp += healingAmount;
    }

    public override void ReApplyBuff() {
        if (skip) {
            skip = false;
            return;
        }

        if (duration == 0)
            return;

        Player.Instance.CurrentHp += healingAmount;
        duration--;
    }

    public override void ClearBuff() {
        
    }

    public override string getDescription() {
        return "Healing Buff\nHealing Amount: " + healingAmount.ToString();
    }
}
