using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    protected Enemy enemy;

    public virtual Enemy Enemy {
        get { return enemy; }
    }

}
