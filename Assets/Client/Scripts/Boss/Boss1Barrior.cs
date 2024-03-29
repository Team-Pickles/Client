using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Barrior : Boss1State
{
    int barriorCount = 5;
    public Boss1Barrior(Boss1 boss) : base(boss)
    {
    }
    public override IEnumerator Start()
    {
        Debug.Log("Barrior start");
        yield return new WaitForSeconds(1.0f);
        yield return Skill();


        //yield return new WaitForSeconds(3.0f);
        //_boss.OnEnd();
        //yield break;
    }
    public override IEnumerator Skill()
    {
        Vector2 barriorPosition = new Vector2(_boss.transform.position.x + 1.0f, _boss.transform.position.y);
        for (int i=0;i < barriorCount;i++)
        {
            GameObject obj = Object.Instantiate(_boss.barrior, barriorPosition, new Quaternion());
            obj.GetComponent<Rigidbody2D>().angularVelocity = 100.0f;
            yield return new WaitForSeconds(2.0f / barriorCount);
        }
        _boss.SetState(new Boss1Rain(_boss));
        yield break;
    }
    public override IEnumerator End()
    {
        yield break;
    }
}
