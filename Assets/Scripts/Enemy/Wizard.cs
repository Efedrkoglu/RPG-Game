using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    protected override void Start() {
        enemyName = "Wizard";
        maxHp = 300;
        damage = 150;
        exp = 1000;
        base.Start();
    }

    public override void PlayTurn(GameObject enemyUnit, GameObject playerUnit) {
        if (!Player.Instance.IsInCombat || Player.Instance.IsDead) return;

        if (CheckAttack()) SetLastDealtDamage(true);
        else SetLastDealtDamage(false);

        SetDamageInfo(GetLastDealtDamage());

        enemyUnit.GetComponent<Animator>().SetTrigger("Attack");
    }
}
