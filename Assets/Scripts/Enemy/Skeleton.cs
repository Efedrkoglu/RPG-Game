using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Start() {
        enemyName = "Skeleton";
        maxHp = 35;
        currentHp = maxHp;
        damage = 100;
        exp = 531;
        base.Start();
    }

}
