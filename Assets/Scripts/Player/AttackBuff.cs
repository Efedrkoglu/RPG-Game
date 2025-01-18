using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuff : Buff
{
    private int attackBonus;
    private bool alreadyCleared;

    public AttackBuff(int duration, int attackBonus) : base(duration, 0) {
        this.attackBonus = attackBonus;
        this.alreadyCleared = false;
        ApplyBuff();
    }

    protected override void ApplyBuff() {
        Player.Instance.Damage += attackBonus;
    }

    public override void ReApplyBuff() {
        if(skip) {
            skip = false;
            return;
        }

        if (duration == 0)
            return;

        Player.Instance.Damage -= attackBonus;
        Player.Instance.Damage += attackBonus;
        duration--;
    }

    public override void ClearBuff() {
        if (alreadyCleared)
            return;

        Player.Instance.Damage -= attackBonus;
        alreadyCleared = true;
    }

    public override string getDescription() {
        return "Attack Buff\nAttack Bonus: " + attackBonus.ToString();
    }
}
