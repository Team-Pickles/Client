using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fragile : MonoBehaviour
{
    private int _posX, _posY;
    private float _x, _y;
    private Vector3Int _position;
    private Tilemap _tilemap;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Bullet(Clone)")
        {
            _x = collision.transform.position.x;
            _y = collision.transform.position.y;
            _posX = (int)Math.Floor(_x);
            _posY = (int)Math.Floor(_y);
            _tilemap = gameObject.GetComponent<Tilemap>();
            for (int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    _position = new Vector3Int((_posX + i), (_posY + j), 0);
                    _tilemap.SetTile(_position, null);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
