using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSBoss2Attack : CSBoss2State
{
    Bounds _moveRegion;
    GameObject _indicator;
    const int _attackCnt = 10;
    public CSBoss2Attack(CSBoss2 boss) : base(boss)
    {
        _moveRegion = GameObject.Find("Boss/MoveRegion").GetComponent<SpriteRenderer>().bounds;
        _indicator = Resources.Load("Prefabs/CSBoss2/CSBoss2AttackIndicator") as GameObject;
    }
    public override IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        yield return Skill();
        yield break;
    }
    public IEnumerator Move()
    {
        float x = Mathf.Floor(Random.Range(_moveRegion.min.x, _moveRegion.max.x));
        float y = _moveRegion.min.y;
        Vector2 towards = new Vector2(x, y);

        while (Vector2.Distance(_boss.transform.position, towards) > 1.0f)
        {
            _boss.transform.position = Vector2.MoveTowards(_boss.transform.position, towards, 0.05f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(3.0f);
        yield break;
    }

    public override IEnumerator Skill()
    {
        int attackCnt = 0;

        while (attackCnt < _attackCnt)
        {
            //yield return Move();
            float x = Mathf.Floor(Random.Range(_moveRegion.min.x, _moveRegion.max.x));
            float y = _moveRegion.min.y;
            Vector2 position = new Vector2(x, y);

            GameObject indicator = Object.Instantiate(_indicator, position, new Quaternion());

            attackCnt++;
            yield return new WaitForSeconds(1.0f);
        }

        _boss.SetState(new CSBoss2Move(_boss));
        yield break;
    }
    public override IEnumerator End()
    {
        yield break;
    }
}
