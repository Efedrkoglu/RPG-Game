using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    protected int duration;
    protected bool skip;
    protected int type;
    
    protected Buff(int duration, int type) {
        this.duration = duration;
        this.type = type;
        this.skip = true;
    }

    public int Duration {
        get { return duration; }
    }

    public int Type {
        get { return type; }
    }

    protected abstract void ApplyBuff();
    public abstract void ReApplyBuff();
    public abstract void ClearBuff();
    public abstract string getDescription();
}