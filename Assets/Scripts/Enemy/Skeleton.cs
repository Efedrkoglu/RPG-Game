using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Start() {
        enemyName = "Skeleton";
        maxHp = 10;
        currentHp = maxHp;
        damage = 5;
    }
}
