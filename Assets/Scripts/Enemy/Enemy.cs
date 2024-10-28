using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
    private string name;
    private int maxHp;
    private int currentHp;
    private int damage;
    private int bounty;
    private int xpPoints;

    public Enemy(string name, int maxHp, int damage, int bounty, int xpPoints) {
        this.name = name;
        this.maxHp = maxHp;
        this.currentHp = maxHp;
        this.damage = damage;
        this.bounty = bounty;
        this.xpPoints = xpPoints;
    }

    public string Name {
        get { return name; }
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
            if (currentHp > maxHp)
                currentHp = maxHp;
        }
    }

    public int Damage {
        get { return damage; }
    }

    public int Bounty {
        get { return bounty; }
    }

    public int XpPoints {
        get { return xpPoints; }
    }
}
