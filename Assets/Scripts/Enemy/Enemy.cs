using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
	private int maxHp;
	private int currentHp;
	private int damage;
	
	public Enemy(int hp, int damage) {
		this.maxHp = hp;
		this.currentHp = maxHp;
		this.damage = damage;
	}

	public void TakeDamage(int val) {
		currentHp -= val;

		if(currentHp <= 0) {
			currentHp = 0;
		}
	}

	public void Heal(int val) {
		currentHp += val;

		if(currentHp > maxHp) {
			currentHp = maxHp;
		}
	}

	public int getDamage() {
		return this.damage;
	}

	public int getMaxHp() {
		return this.maxHp;
	}

	public int getCurrentHp() {
		return this.currentHp;
	}
}
