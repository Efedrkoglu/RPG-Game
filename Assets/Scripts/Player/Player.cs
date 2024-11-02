using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	private int maxHp;
	private int currentHp;
	private int damage;
	private int gold;

	public Player(int hp, int damage, int gold) {
		this.maxHp = hp;
		this.currentHp = this.maxHp;
		this.damage = damage;
		this.gold = gold;
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
		set { damage = value; }
	}

	public int Gold {
		get { return gold; }
		set { gold = value; }
	}
}
