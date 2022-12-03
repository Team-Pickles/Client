using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grenade : Skill
{
    private GameObject _tilemapFragile, _tilemapBlock;
    private PlayerMoveManager _pmm;
    private SpellManager _sm;
    private int _x, _y;

    public override void OnChange()
    {
        //_player = GameObject.Find("Player");
        //_firePoint = GameObject.Find("FirePoint");
        _sm = _player.GetComponentInChildren<SpellManager>();
        _tilemapFragile = GameObject.Find("Tilemap_fragile");
        _tilemapBlock = GameObject.Find("Tilemap_block");
        _pmm = _player.GetComponent<PlayerMoveManager>();
    }
    public override IEnumerator OnItemUse()
    {
        if (_pmm.GrenadeCount > 0)
        {
            _pmm.DecreaseGrenade();
            GameObject grenade;
            int isFliped = _player.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            grenade = UnityEngine.Object.Instantiate(_pmm.grenadePrefab, _firePoint.transform.position, new Quaternion());
            grenade.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(100.0f * isFliped, 500.0f));
            grenade.transform.GetComponent<Rigidbody2D>().angularVelocity = 300.0f;

            yield return new WaitForSeconds(5.0f);

            _x = (int)Math.Floor(grenade.transform.position.x);
            _y = (int)Math.Floor(grenade.transform.position.y);
            for (int i=-2;i<= 2;i++)
            {
                for (int j=-2;j<=2;j++)
                {
                    Vector3Int position = new Vector3Int(_x + i, _y + j, 0);
                    _tilemapFragile.GetComponent<Tilemap>().SetTile(position, null);
                    _tilemapBlock.GetComponent<Tilemap>().SetTile(position, null);
                }
            }

            UnityEngine.Object.Destroy(grenade);
        }
        if (_pmm.GrenadeCount == 0)
            _sm.ChangeSkill(new Vacuum());
        yield return 0;
    }
}
