using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public event Action OnEnemyHurt;

    public void HurtEnemy() {
        OnEnemyHurt?.Invoke();
    }
}
