using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonUnit : EnemyUnit
{
	public override Enemy Enemy {
		get {
			if (enemy == null)
				enemy = new Skeleton();

			return base.Enemy;
		}
	}
}
