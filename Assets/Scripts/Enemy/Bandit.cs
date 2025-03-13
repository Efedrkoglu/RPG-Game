using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    protected override void Start() {
        enemyName = "Bandit";
        maxHp = 20;
        damage = 7;
        exp = 200;
        base.Start();
    }

    public override void PlayTurn(GameObject enemyUnit, GameObject playerUnit) {
        if(!Player.Instance.IsInCombat || Player.Instance.IsDead) return;

        Player.Instance.CurrentHp -= damage;
        enemyUnit.GetComponent<Animator>().SetTrigger("Attack");
        if(Player.Instance.CurrentHp > 0) playerUnit.GetComponent<Animator>().SetTrigger("Hurt");
    }

}
