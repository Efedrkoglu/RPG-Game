using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonVariation : Enemy
{
    protected override void Start() {
        enemyName = "Skeleton";
        maxHp = 30;
        damage = 5;
        exp = 120;
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
