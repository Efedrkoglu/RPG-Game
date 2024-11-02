using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    protected override void Start() {
        enemyName = "Bandit";
        maxHp = 15;
        currentHp = maxHp;
        damage = 5;
    }

}
