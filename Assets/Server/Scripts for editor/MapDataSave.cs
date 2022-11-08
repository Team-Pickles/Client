using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;

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

    // TYPE 2: 배열로 받는 경우
    [SerializeField] GameObject[] items;
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] enemys;

    [SerializeField] private string filePath = "MapData/";
    [SerializeField] string fileName = "MyMap";

    private Dictionary<Vector3, DataClass> infos;
    private string fullFilePath;

    private List<string> TileSpriteTypes = System.Enum.GetNames(typeof(TileType)).ToList<string>();

    private void GetItemInfo()
    {
        // TYPE 1: 그룹으로 받는 경우
        // if(itemGroup != null) {
        //     int itemCnt = itemGroup.transform.childCount;
        //     for (int i = 0; i < itemCnt; ++i) {
        //         GameObject item = itemGroup.transform.GetChild(i).gameObject;
        //         Vector3 pos = item.transform.position;
        //         var itemSprite = item.GetComponent<Sprite>().name;
        //         if(GoldSpriteTypes.Contains(itemSprite))
        //             infos.Add(pos, new DataClass((int)InfoTypes.item, pos, (int)ItemTypes.GoldCoin));
        //         else if(SilverSpriteTypes.Contains(itemSprite))
        //             infos.Add(pos, new DataClass((int)InfoTypes.item, pos, (int)ItemTypes.SilverCoin));
        //         else
        //             infos.Add(pos, new DataClass((int)InfoTypes.item, pos, (int)ItemTypes.BronzeCoin));
        //     }
        // }

        // TYPE 2: 배열로 받는 경우
    }

    private void GetTileInfo()
    {
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
                Debug.Log($" {TileSpriteTypes.IndexOf(tileSprite.name)}, {tileSprite.name}");

                infos.Add(pos, new DataClass((int)TileSpriteTypes.IndexOf(tileSprite.name), pos, TileSpriteTypes.IndexOf(tileSprite.name)));
            }
        }
    }

    private void GetEnemyInfo()
    {
        // TYPE 1: 그룹으로 받는 경우
        // if(enemyGroup != null) {
        //     int enemyCnt = enemyGroup.transform.childCount;
        //     for (int i = 0; i < enemyCnt; ++i) {
        //         GameObject enemy = enemyGroup.transform.GetChild(i).gameObject;
        //         Vector3 pos = enemy.transform.position;
        //         var enemySprite = enemy.GetComponent<Sprite>().name;
        //         if(EnemySpriteTypes.Contains(enemySprite))
        //             infos.Add(pos, new DataClass((int)InfoTypes.enemy, pos, (int)EnemyTypes.Enemy));
        //     }
        // }

        // TYPE 2: 배열로 받는 경우
    }

    private void GetPlayerInfo()
    {
        // TYPE 1: 그룹으로 받는 경우
        // if(playerGroup != null) {
        //     int playerCnt = playerGroup.transform.childCount;
        //     for (int i = 0; i < playerCnt; ++i) {
        //         GameObject player = playerGroup.transform.GetChild(i).gameObject;
        //         Vector3 pos = player.transform.position;
        //         var playerSprite = player.GetComponent<Sprite>().name;
        //         if(PlayerSpriteTypes.Contains(playerSprite))
        //             infos.Add(pos, new DataClass((int)InfoTypes.player, pos, (int)PlayerTypes.player));
        //     }
        // }

        // TYPE 2: 배열로 받는 경우
    }

    public void Save()
    {

        infos = new Dictionary<Vector3, DataClass>();
        fullFilePath = filePath + fileName;

        GetTileInfo();
        GetItemInfo();
        GetPlayerInfo();
        GetEnemyInfo();

        string toJson = JsonUtility.ToJson(new Serialization<Vector3, DataClass>(infos));
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
