using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;
using System;
/*
    Made by limmon029 
 */
public class MapDataSave : MonoBehaviour
{
    [SerializeField] GameObject tileMapGrid;
    // TYPE 1: 그룹으로 받는 경우
    // [SerializeField] GameObject itemGroup;
    // [SerializeField] GameObject playerGroup;
    // [SerializeField] GameObject enemyGroup;
    [SerializeField] private string filePath = "MapData/";
    [SerializeField] string fileName = "MyMap";

    public GameObject backGround;
    private Dictionary<int, DataClass> infos;
    private string fullFilePath;

    //private List<string> TileSpriteTypes = System.Enum.GetNames(typeof(TileType)).ToList<string>();



    private int key;

    private void GetMapInfo()
    {
        key = 1;
        object tileEnum;
        int tileMapCnt = tileMapGrid.transform.childCount;
        for (int i = 0; i < tileMapCnt; ++i)
        {
            Tilemap tileMap = tileMapGrid.transform.GetChild(i).gameObject.GetComponent<Tilemap>();
            foreach (Vector3Int _pos in tileMap.cellBounds.allPositionsWithin)
            {
                Vector3 pos = _pos + new Vector3(0.5f, 0.5f, 0);

                if (!tileMap.HasTile(_pos))
                    continue;

                var tile = tileMap.GetTile<TileBase>(_pos);
                var tileSprite = tileMap.GetSprite(_pos);
                tileEnum = Enum.Parse(typeof(TileType), tileSprite.name);
                infos.Add(key, new DataClass((int)tileEnum / 100, pos, (int)tileEnum));
                ++key;
            }
        }
        var backGroundName = backGround.GetComponent<SpriteRenderer>().sprite.name;
        tileEnum = Enum.Parse(typeof(TileType), backGroundName);
        infos.Add(key, new DataClass((int)tileEnum / 100, new Vector3(0, 0, 0), (int)tileEnum));
    }

    public void Save()
    {

        infos = new Dictionary<int, DataClass>();
        fullFilePath = filePath + fileName;

        GetMapInfo();
        key = 0;
        string toJson = JsonUtility.ToJson(new Serialization<int, DataClass>(infos));
        int num = 0;

        Debug.Log($"{filePath}");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        while (File.Exists(fullFilePath + ".json"))
        {
            fullFilePath = fullFilePath + "_" + num;
        }
        File.WriteAllText(fullFilePath + ".json", toJson);

        Debug.Log("save done");
    }
}
