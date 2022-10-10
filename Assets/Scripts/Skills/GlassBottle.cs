using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlassBottle : Skill
{
    GameObject _player;

    public override void OnChange()
    {
        _player = GameObject.Find("Player");
        Debug.Log("GlassBottle equib");
    }
    public override void OnStart()
    {

    }
    public override void OnSkill()
    {

    }
    public override IEnumerator OnFire()
    {
        PlayerMoveManager pmm = _player.GetComponent<PlayerMoveManager>();
        bool flip = _player.GetComponent<SpriteRenderer>().flipX;
        //if (pmm.BulletCount > 0)
        {
            //pmm.DecreaseBullet();
            Vector3 position;
            GameObject glassbottle;
            if(!flip)
            {
                position = new Vector3(_player.transform.position.x + _player.transform.localScale.x / 2.0f, _player.transform.position.y + _player.transform.localScale.y / 2.0f * 1.5f, 0);
                glassbottle = Object.Instantiate(pmm.glassbottlePrefab, position, new Quaternion());

                for (int i = 0; i < 9; i++)
                {
                    glassbottle.transform.GetChild(i).GetComponent<Rigidbody2D>().AddForce(new Vector2(400.0f, 300.0f));
                    glassbottle.transform.GetChild(i).GetComponent<Rigidbody2D>().angularVelocity = 300.0f;
                }
            }
            else
            {
                position = new Vector3(_player.transform.position.x - _player.transform.localScale.x / 2.0f, _player.transform.position.y + _player.transform.localScale.y / 2.0f * 1.5f, 0);
                glassbottle = Object.Instantiate(pmm.glassbottlePrefab, position, new Quaternion());

                for (int i = 0; i < 9; i++)
                {
                    glassbottle.transform.GetChild(i).GetComponent<Rigidbody2D>().AddForce(new Vector2(-400.0f, 300.0f));
                    glassbottle.transform.GetChild(i).GetComponent<Rigidbody2D>().angularVelocity = 300.0f;
                }
            }
            
            //yield return new WaitForSeconds(5.0f);

            //Object.Destroy(bullet);
        }
        yield return 0;
    }
    public override void OnEnd()
    {

    }
}
