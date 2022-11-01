using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Idle : Boss1State
{
    public Boss1Idle(Boss1 boss) :base(boss)
    {
    }
    public override IEnumerator Start()
    {
        Debug.Log("Idle start");
        yield return new WaitForSeconds(1.0f);
        yield return Skill();
        yield break;
    }
    public override IEnumerator Skill()
    {
        Debug.Log("Idle skill start");
        yield return new WaitForSeconds(1.0f);
        _boss.SetState(new Boss1Rain(_boss));
        yield break;
    }
    public override IEnumerator End()
    {
        yield break;
    }
}
