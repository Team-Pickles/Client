using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss1State
{
    protected readonly Boss1 _boss;
    public Boss1State(Boss1 boss)
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
