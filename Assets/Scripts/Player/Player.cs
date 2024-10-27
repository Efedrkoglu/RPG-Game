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

	public int getMaxHp() {
		return this.maxHp;
	}

	public int getCurrentHp() {
		return this.currentHp;
	}

	public int getDamage() {
		return this.damage;
	}
}
