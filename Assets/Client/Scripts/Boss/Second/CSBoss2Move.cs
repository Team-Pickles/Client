using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSBoss2Move : CSBoss2State
{
    Bounds _moveRegion;
    public CSBoss2Move(CSBoss2 boss) : base(boss)
    {
        _moveRegion = GameObject.Find("Boss/MoveRegion").GetComponent<SpriteRenderer>().bounds;
    }
    public override IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        yield return Skill();
        yield break;
    }
    public IEnumerator Move()
    {
        float x = Random.Range(_moveRegion.min.x, _moveRegion.max.x);
        float y = Random.Range(_moveRegion.min.y, _moveRegion.max.y);
        Vector2 towards = new Vector2(x, y);

        while (Vector2.Distance(_boss.transform.parent.transform.position, towards) > 1.0f)
        {
            _boss.transform.parent.transform.position = Vector2.MoveTowards(_boss.transform.parent.transform.position, towards, 0.07f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(3.0f);
        yield break;
    }

    public override IEnumerator Skill()
    {
        yield return Move();


        _boss.SetState(new CSBoss2Attack(_boss));
        yield break;
    }
    public override IEnumerator End()
    {
        yield break;
    }
}
