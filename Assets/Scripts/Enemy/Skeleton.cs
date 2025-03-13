using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Start() {
        enemyName = "Skeleton";
        maxHp = 50;
        damage = 10;
        exp = 531;
        base.Start();
    }

    public override void PlayTurn(GameObject enemyUnit, GameObject playerUnit) {
        if(!Player.Instance.IsInCombat || Player.Instance.IsDead) return;

        if(turnCount % 2 == 0 && turnCount != 0) {
            currentHp += 10;
            Player.Instance.CurrentHp -= damage;
            enemyUnit.GetComponent<Animator>().SetTrigger("Attack");
            if(Player.Instance.CurrentHp > 0) playerUnit.GetComponent<Animator>().SetTrigger("Hurt");
        }
        else {
            Player.Instance.CurrentHp -= damage;
            enemyUnit.GetComponent<Animator>().SetTrigger("Attack");
            if(Player.Instance.CurrentHp > 0) playerUnit.GetComponent<Animator>().SetTrigger("Hurt");
        }
        turnCount++;
    }

}
