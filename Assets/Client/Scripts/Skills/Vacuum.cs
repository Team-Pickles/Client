using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Vacuum : Skill
{
    private GameObject _player;
    private GameObject _firePoint;
    private GameObject _tilemapFragile, _tilemapBlock;
    public override void OnChange()
    {
        _player = GameObject.Find("Player");
        _firePoint = GameObject.Find("FirePoint");
        _tilemapFragile = GameObject.Find("Tilemap_fragile");
        _tilemapBlock = GameObject.Find("Tilemap_block");
    }
    public override void OnStart()
    {
        _player = GameObject.Find("Player");
        _firePoint = GameObject.Find("FirePoint");
    }
    public override void OnSkill()
    {
        bool flip = _player.transform.localScale.x > 0 ? false : true;//_player.GetComponent<SpriteRenderer>().flipX;
        Vector2 rayOrigin = _firePoint.transform.position;//new Vector2(_player.transform.position.x + _player.transform.localScale.x / 2.0f * 1.01f * (flip == false ? 1 : -1), _player.transform.position.y - _player.transform.localScale.y/4);
        
        float length = 8.0f;

        for (int i = -5; i <= 5; i++)
        {
            Vector2 rayDirection = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad) * (flip == false ? 1 : -1), Mathf.Sin(i * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, length);

            if (hit.collider != null)
            {
                Debug.DrawLine(rayOrigin, hit.point, Color.green);
                if (hit.transform.CompareTag("trash"))
                {
                    hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce((hit.point - rayOrigin).normalized * -3.0f);
                    hit.transform.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 200.0f;
                }
            }
        }
    }
    public override IEnumerator OnBulletFire()
    {
        PlayerMoveManager pmm = _player.GetComponent<PlayerMoveManager>();

        if (pmm.BulletCount > 0)
        {
            pmm.DecreaseBullet();

            GameObject bullet;
            int isFliped = _player.transform.localScale.x > 0 ? 1 : -1;
            bullet = UnityEngine.Object.Instantiate(pmm.bulletPrefab, _firePoint.transform.position, new Quaternion());
            bullet.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(500.0f * isFliped, 0.0f));
            bullet.transform.GetComponent<Rigidbody2D>().angularVelocity = 500.0f;

            Vector3 scale = bullet.transform.GetComponent<Rigidbody2D>().transform.localScale;
            bullet.transform.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(isFliped * scale.x, scale.y, scale.z);
            yield return new WaitForSeconds(2.0f);
            UnityEngine.Object.Destroy(bullet);
        }
    }
    public override IEnumerator OnGrenadeFire()
    {
        PlayerMoveManager pmm = _player.GetComponent<PlayerMoveManager>();

        if (pmm.GrenadeCount > 0)
        {
            pmm.DecreaseGrenade();
            GameObject grenade;
            int isFliped = _player.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            grenade = UnityEngine.Object.Instantiate(pmm.grenadePrefab, _firePoint.transform.position, new Quaternion());
            grenade.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(100.0f * isFliped, 500.0f));
            grenade.transform.GetComponent<Rigidbody2D>().angularVelocity = 300.0f;

            yield return new WaitForSeconds(5.0f);

            int x = (int)Math.Floor(grenade.transform.position.x);
            int y = (int)Math.Floor(grenade.transform.position.y);
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    Vector3Int position = new Vector3Int(x + i, y + j, 0);
                    _tilemapFragile.GetComponent<Tilemap>().SetTile(position, null);
                    _tilemapBlock.GetComponent<Tilemap>().SetTile(position, null);
                }
            }

            UnityEngine.Object.Destroy(grenade);
        };
        yield return 0;
    }
    public override void OnEnd()
    {
        Debug.Log("Vacuum::End");
    }
}