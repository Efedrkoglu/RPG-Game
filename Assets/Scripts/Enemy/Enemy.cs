using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected string enemyName;
    protected int maxHp;
    protected int currentHp;
    protected int damage;

    protected virtual void Start() {

    }

    public string EnemyName {
        get { return enemyName; }
    }

    public int MaxHp {
        get { return maxHp; }
    }

    public int CurrentHp {
        get { return currentHp; }
        set {
            currentHp = value;

            if (currentHp < 0)
                currentHp = 0;
            else if (currentHp > maxHp)
                currentHp = maxHp;
        }
    }

    public int Damage {
        get { return damage; }
        set { damage = value; }
    }
}
