using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;
    private GameObject _player;

    private List<List<int>> _map = new List<List<int>>();
    private int _mapX, _mapY;
    private Vector3Int _playerPosition;
    public Vector3Int PlayerPosition
    {
        get { return _playerPosition; }
    }

    public Vector3Int GetTopLeftBasePosition(Vector3 position)
    {
        Vector3Int temp = tilemap.WorldToCell(position);
        return new Vector3Int(temp.x + tilemap.size.x, tilemap.size.y - temp.y);
    }
    IEnumerator SetPlayerPosition()
    {
        Vector3Int oldPosition;
        _playerPosition = GetTopLeftBasePosition(_player.transform.position);
        oldPosition = _playerPosition;
        while (true)
        {
            _map[oldPosition.y][oldPosition.x] = 0;
            _playerPosition = GetTopLeftBasePosition(_player.transform.position);
            _map[_playerPosition.y][_playerPosition.x] = 5;
            oldPosition = _playerPosition;

            yield return new WaitForSeconds(1.0f);
        }
    }
    void Start()
    {
        _player = GameObject.Find("Player");
        _playerPosition = GetTopLeftBasePosition(_player.transform.position);
        _mapX = tilemap.size.x * 2 + 1;
        _mapY = tilemap.size.y * 2 + 1;
        for (int i = tilemap.size.y; i >= -tilemap.size.y; i--)
        {
            _map.Add(new List<int>());
            for (int j = -tilemap.size.x; j <= tilemap.size.x; j++)
            {
                _map[tilemap.size.y-i].Add(tilemap.HasTile(new Vector3Int(j, i, 0)) == true ? 1 : 0);
            }
        }
        StartCoroutine(SetPlayerPosition());
        /*
        for (int i=0;i<_mapY;i++)
        {
            string s = "";
            for (int j = 0; j < _mapX; j++)
                s += _map[i][j] + " ";
            Debug.Log(s);
        }
        */
    }
    void Update()
    {
    
        
    }
}
