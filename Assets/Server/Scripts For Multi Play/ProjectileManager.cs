using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProjectileManager : MonoBehaviour
{
    public int id;
    public MeshRenderer projectileModel;
    public GameObject explosionPrefab;
    private GameObject _tilemapFragile, _tilemapBlock;

    public void Initialize(int _id)
    {
        id = _id;
        _tilemapFragile = GameObject.Find("Fragile");
        _tilemapBlock = GameObject.Find("Block");
    }

    public void Explode(Vector3 _position)
    {
        //transform.position = _position;
        int _x = (int)Math.Floor(transform.position.x);
        int _y = (int)Math.Floor(transform.position.y);
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                Vector3Int position = new Vector3Int(_x + i, _y + j, 0);
                _tilemapFragile.GetComponent<Tilemap>().SetTile(position, null);
                _tilemapBlock.GetComponent<Tilemap>().SetTile(position, null);
                Debug.Log(position);
            }
        }
        GameManagerInServer.projectiles.Remove(id);
        Destroy(gameObject);
    }
}

