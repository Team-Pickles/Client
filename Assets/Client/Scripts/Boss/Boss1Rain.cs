using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Rain : Boss1State
{
    const int attackCount = 20;
    public Boss1Rain(Boss1 boss) : base(boss)
    {
    }
    public override IEnumerator Start()
    {
        Debug.Log("Rain start");
        yield return new WaitForSeconds(1.0f);
        yield return Skill();
        yield break;
    }
    public override IEnumerator Skill()
    {
        Debug.Log("Rain skill start");
        yield return new WaitForSeconds(1.0f);
        SpriteRenderer sr1 = _boss.rainStart.GetComponent<SpriteRenderer>();

        int cnt = 0;
        while(cnt < attackCount)
        {
            float startX = Random.Range(sr1.bounds.min.x, sr1.bounds.max.x);
            float startY = Random.Range(sr1.bounds.min.y, sr1.bounds.max.y);
            float endX = Random.Range(sr1.bounds.min.x, sr1.bounds.max.x);
            float endY = Random.Range(sr1.bounds.min.y, sr1.bounds.max.y);

            float rand = Random.Range(0.0f, 1.0f);
            GameObject obj;
            if (rand <= 0.2f)
            {
                obj = Object.Instantiate(_boss.trash, new Vector2(startX, startY), new Quaternion());

            }
            else
            {
                obj = Object.Instantiate(_boss.enemy, new Vector2(startX, startY), new Quaternion());
            }
            obj.GetComponent<Rigidbody2D>().angularVelocity = 3.0f;
            if (_boss.hp == 0)
            {
                yield return new WaitForSeconds(3.0f);
                yield return End();
                break;
            }

            cnt++;
            yield return new WaitForSeconds(0.5f);
        }
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
