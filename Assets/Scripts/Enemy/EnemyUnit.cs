using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    private Enemy enemy;

    public event Action<bool> OnPlayerHurt;

    public void HurtPlayer() {
        if(enemy.GetLastDealtDamage() == 0) OnPlayerHurt?.Invoke(false);
        else OnPlayerHurt?.Invoke(true);
    }

    public void setEnemy(Enemy enemy) {
        this.enemy = enemy;
    }
}
