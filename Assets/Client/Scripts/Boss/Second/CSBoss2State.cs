using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CSBoss2State
{
    protected readonly CSBoss2 _boss;
    public CSBoss2State(CSBoss2 boss)
    {
        _boss = boss;
    }
    public virtual IEnumerator Start()
    {
        yield break;
    }
    public virtual IEnumerator Skill()
    {
        yield break;
    }
    public virtual IEnumerator End()
    {
        yield break;
    }
}
