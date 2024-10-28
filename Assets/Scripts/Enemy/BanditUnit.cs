using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditUnit : EnemyUnit
{
	public override Enemy Enemy {
		get {
			if (enemy == null)
				enemy = new Bandit();

			return base.Enemy;
		}
	}
}
