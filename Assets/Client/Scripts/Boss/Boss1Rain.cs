using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Rain : Boss1State
{
    const int attackCount = 20;
    GameObject attackIndicator;
    SpriteRenderer sr;
    public Boss1Rain(Boss1 boss) : base(boss)
    {
    }
    public override IEnumerator Start()
    {
        attackIndicator = Resources.Load("Prefabs/Boss1/AttackIndicator") as GameObject;
        sr = _boss.rainStart.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(1.0f);
        yield return Skill();
    }
    public override IEnumerator Skill()
    {
        int cnt = 0;
        float x, y;
        // 20�� ���� �ʴ� 1ȸ ����
        while(cnt < attackCount)
        {
            x = Random.Range(sr.bounds.min.x, sr.bounds.max.x);
            y = Random.Range(sr.bounds.min.y, sr.bounds.max.y);

            Object.Instantiate(attackIndicator, new Vector2(x, y), new Quaternion());

            cnt++;
            yield return new WaitForSeconds(1.0f);
        }

        for (int i = 0; i < 5; i++)
        {
            x = Random.Range(sr.bounds.min.x, sr.bounds.max.x);
            y = sr.bounds.max.y;
            Object.Instantiate(_boss.trash, new Vector2(x, y), new Quaternion());
        }
        // 3�� ����
        yield return new WaitForSeconds(3.0f);
        
        // �ٸ� ��������
        if (_boss.hp > 0)
            _boss.SetState(new Boss1Barrior(_boss));
        //_boss.SetState(new Boss1Idle(_boss));
        //yield break;
    }
    public override IEnumerator End()
    {
        GameObject player = GameObject.Find("Player");
        GameObject lv2StartPoint = GameObject.Find("Lv2StartPoint");
        player.transform.position = lv2StartPoint.transform.position;
        yield break;
    }
}
