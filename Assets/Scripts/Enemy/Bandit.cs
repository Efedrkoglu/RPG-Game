using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    protected override void Start() {
        enemyName = "Bandit";
        maxHp = 20;
        currentHp = maxHp;
        damage = 5;
    }

}
