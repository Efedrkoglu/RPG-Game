using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    protected int duration;
    protected bool skip;
    
    protected Buff(int duration) {
        this.duration = duration;
        this.skip = true;
    }

    public int Duration {
        get { return duration; }
    }

    protected abstract void ApplyBuff();
    public abstract void ReApplyBuff();
    public abstract void ClearBuff();
    public abstract string Description();
}