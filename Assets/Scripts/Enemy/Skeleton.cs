using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Start() {
        enemyName = "Skeleton";
        maxHp = 50;
        damage = 1;
        exp = 531;
        base.Start();
    }

    public override void PlayTurn(GameObject enemyUnit, GameObject playerUnit) {
        if(!Player.Instance.IsInCombat || Player.Instance.IsDead) return;

        if(turnCount % 2 == 0 && turnCount != 0) {
            currentHp += 15;

            if(CheckAttack()) SetLastDealtDamage(true);
            else SetLastDealtDamage(false);

            SetDamageInfo(GetLastDealtDamage() * 2);

            enemyUnit.GetComponent<Animator>().SetTrigger("Attack");
        }
        else {
            if(CheckAttack()) SetLastDealtDamage(true);
            else SetLastDealtDamage(false);

            SetDamageInfo(GetLastDealtDamage() * 2);

            enemyUnit.GetComponent<Animator>().SetTrigger("Attack");
        }
        turnCount++;
    }

}
