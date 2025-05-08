using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    protected override void Start() {
        enemyName = "Bandit Leader";
        maxHp = 40;
        damage = 20;
        exp = 30;
        base.Start();
    }

    public override void PlayTurn(GameObject enemyUnit, GameObject playerUnit) {
        if(!Player.Instance.IsInCombat || Player.Instance.IsDead) return;

        if(CheckAttack()) SetLastDealtDamage(true);
        else SetLastDealtDamage(false);

        SetDamageInfo(GetLastDealtDamage());

        enemyUnit.GetComponent<Animator>().SetTrigger("Attack");
    }

}
