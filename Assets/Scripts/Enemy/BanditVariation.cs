using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditVariation : Enemy
{
    protected override void Start() {
        enemyName = "Bandit";
        maxHp = 25;
        damage = 10;
        exp = 25;
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
