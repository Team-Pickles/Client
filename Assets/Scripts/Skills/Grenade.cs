using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Skill
{
    GameObject _player;
    public override void OnChange()
    {
        _player = GameObject.Find("Player");
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
        //if (pmm.BulletCount > 0)
        {
            //pmm.DecreaseBullet();
            GameObject bullet = Object.Instantiate(pmm.bulletPrefab, new Vector3(_player.transform.position.x + _player.transform.localScale.x / 2.0f * 1.1f, _player.transform.position.y, 0), new Quaternion());

            bullet.transform.GetComponent<CircleCollider2D>().isTrigger = false;
            bullet.transform.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            bullet.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(100.0f, 500.0f));
            bullet.transform.GetComponent<Rigidbody2D>().angularVelocity = 300.0f;
            yield return new WaitForSeconds(5.0f);
            Object.Destroy(bullet);
        }
        yield return 0;
    }
    public override void OnEnd()
    {

    }
}
