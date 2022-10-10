using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grenade : Skill
{
    GameObject _player;
    GameObject _tilemap;

    public override void OnChange()
    {
        _player = GameObject.Find("Player");
        _tilemap = GameObject.Find("Tilemap_ground");
        Debug.Log("Grenade equib");
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
            GameObject grenade;
            if(!flip)
            {
                grenade = Object.Instantiate(pmm.grenadePrefab, new Vector3(_player.transform.position.x + _player.transform.localScale.x / 2.0f * 1.1f, _player.transform.position.y, 0), new Quaternion());
                grenade.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(100.0f, 500.0f));
                grenade.transform.GetComponent<Rigidbody2D>().angularVelocity = 300.0f;
            }
            else
            {
                grenade = Object.Instantiate(pmm.grenadePrefab, new Vector3(_player.transform.position.x - _player.transform.localScale.x / 2.0f * 1.1f, _player.transform.position.y, 0), new Quaternion());
                grenade.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100.0f, 500.0f));
                grenade.transform.GetComponent<Rigidbody2D>().angularVelocity = 300.0f;
            }

            yield return new WaitForSeconds(5.0f);

            for (int i=-3;i<=3;i++)
            {
                for (int j=-3;j<=3;j++)
                {
                    Vector3Int position = new Vector3Int((int)grenade.transform.position.x + i, (int)grenade.transform.position.y + j, 0);
                    _tilemap.GetComponent<Tilemap>().SetTile(position, null);
                }
            }

            Object.Destroy(grenade);
        }
        yield return 0;
    }
    public override void OnEnd()
    {

    }
}
