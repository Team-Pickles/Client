using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;
using System;
using UnityEngine.Networking;
using System.Collections;

[Serializable]
public class SaveDataClass
{
    public string map_info;
    public string map_tag;
    public int map_grade = 1;
    public int map_difficulty = 2;
    public string map_maker;
}

public class MapDataSave : MonoBehaviour
{
    [SerializeField] Tilemap tileMap;
    [SerializeField] GameObject[] items;
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] enemys;
    [SerializeField] TileBase[] tileBase;

    [SerializeField] string filePath = "MapData/";
    [SerializeField] string fileName = "MyMap";
    [SerializeField] string map_tag;

    Dictionary<Vector3, DataClass> infos;
    private string fullFilePath;
    private List<string> TileSpriteTypes = System.Enum.GetNames(typeof(TileTypes)).ToList<string>();

    private void GetInfos() {
        foreach (Vector3Int _pos in tileMap.cellBounds.allPositionsWithin) {
            if(!tileMap.HasTile(_pos))
                continue;

            var tile = tileMap.GetTile<TileBase>(_pos);
            var tileSprite = tileMap.GetSprite(_pos);
            infos.Add(new Vector3(_pos.x, _pos.y, _pos.z), new DataClass((int)InfoTypes.tile, new Vector3(_pos.x, _pos.y, _pos.z), TileSpriteTypes.IndexOf(tileSprite.name) - 1));
        }
        foreach(GameObject _item in items)
        {
            var itemSprite = _item.GetComponent<SpriteRenderer>().sprite;
            Vector3 _pos = _item.transform.position;
            int spriteId = TileSpriteTypes.IndexOf(itemSprite.name) - TileSpriteTypes.IndexOf("Item") + (int)TileTypes.Item;
            infos.Add(_pos, new DataClass((int)InfoTypes.item, _pos, spriteId));
        }
        foreach(GameObject _enemy in enemys)
        {
            var enemySprite = _enemy.GetComponent<SpriteRenderer>().sprite;
            Vector3 _pos = _enemy.transform.position;
            infos.Add(_pos, new DataClass((int)InfoTypes.enemy, _pos, 200));
        }
        foreach(GameObject _player in players)
        {
            var playerSprite = _player.GetComponent<SpriteRenderer>().sprite;
            Vector3 _pos = _player.transform.position;
            infos.Add(_pos, new DataClass((int)InfoTypes.player, _pos, 300));
        }
    }

    public void Save(bool _forFile) {
        
        infos = new Dictionary<Vector3, DataClass>();

        GetInfos();

        string toJson = JsonUtility.ToJson(new Serialization<Vector3, DataClass>(infos));

        SaveDataClass _forSend = new SaveDataClass();
        _forSend.map_info = toJson;
        _forSend.map_tag = map_tag;
        _forSend.map_maker = "userId";

        string _forSendJson = JsonUtility.ToJson(_forSend);

        if(_forFile) {
            int num = 0;
            fullFilePath = filePath + fileName;

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            while(File.Exists(fullFilePath + ".json")) {
                fullFilePath = fullFilePath + "_" + num;
            }

            File.WriteAllText(fullFilePath + ".json", toJson);
        } else {
            StartCoroutine(MapSaveProcess(_result => {
                if(_result)
                {Debug.Log("SAVE");}
            }));

            IEnumerator MapSaveProcess(Action<bool> ResultHandler)
            {
                using (UnityWebRequest request = UnityWebRequest.Put("http://localhost:3001/api/map/apply", _forSendJson))
                {
                    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(_forSendJson);

                    request.uploadHandler.Dispose();

                    request.SetRequestHeader("Content-Type", "application/json");

                    request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                    request.downloadHandler = new DownloadHandlerBuffer();

                    yield return request.SendWebRequest();
                    string _result = request.downloadHandler.text;

                    if (request.error != null)
                    {
                        Debug.Log(request.error);
                    }
                    else
                    {
                        Debug.Log(_result);
                        ResultHandler(true);
                    }

                    request.uploadHandler.Dispose();
                    request.downloadHandler.Dispose();
                    request.Dispose();
                }
            }
        }
        //
        Debug.Log("save done");
    }
}
