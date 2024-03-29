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

public class LocalMapDataSaver : MonoBehaviour
{
    [SerializeField] Tilemap[] tileMaps;
    [SerializeField] GameObject[] items;
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] enemys;
    [SerializeField] TileBase[] tileBase;
    public GameObject backGround;

    [SerializeField] string filePath = "MapData/";
    [SerializeField] string fileName = "MyMap";
    [SerializeField] string map_tag;

    Dictionary<int, DataClass> infos;
    private string fullFilePath;
    private string apiUrl = "http://localhost:3001/";
    private int key;
    private void GetInfos() {
        key = 1;
        object tileEnum;
        Dictionary<string, Vector3> _size = new Dictionary<string, Vector3>();
        foreach(Tilemap _now in tileMaps)
        {
            foreach (Vector3Int _pos in _now.cellBounds.allPositionsWithin)
            {
                if(!_now.HasTile(_pos))
                    continue;

                var tile = _now.GetTile<TileBase>(_pos);
                var tileSprite = _now.GetSprite(_pos);
                tileEnum = Enum.Parse(typeof(TileType), tileSprite.name);
                infos.Add(key, new DataClass((int)tileEnum / 100, _pos, (int)tileEnum));
                ++key;
            }
            if(_size.TryAdd("MIN", new Vector3(_now.cellBounds.xMin, _now.cellBounds.yMin, 0)))
            {
                _size.Add("MAX", new Vector3(_now.cellBounds.xMax, _now.cellBounds.yMax, 0));
            }
            else
            {
                _size["MIN"] = new Vector3(Math.Min(_size["MIN"].x, _now.cellBounds.xMin), Math.Min(_size["MIN"].y, _now.cellBounds.yMin));
                _size["MAX"] = new Vector3(Math.Max(_size["MAX"].x, _now.cellBounds.xMax), Math.Max(_size["MAX"].y, _now.cellBounds.yMax));
            }
        }
        
        foreach(GameObject _item in items)
        {
            var itemSprite = _item.GetComponent<SpriteRenderer>().sprite;
            Vector3 _pos = _item.transform.position;
            tileEnum = Enum.Parse(typeof(TileType), itemSprite.name);
            infos.Add(key, new DataClass((int)tileEnum / 100, _pos, (int)tileEnum));
            ++key;
        }
        foreach(GameObject _enemy in enemys)
        {
            var enemySprite = _enemy.GetComponent<SpriteRenderer>().sprite;
            Vector3 _pos = _enemy.transform.position;
            if(enemySprite.name.Contains("can"))
            {
                tileEnum = TileType.can_stand_Sheet_0;
            }
            else
            {
                tileEnum = TileType.Enemy + 1;
            }
            infos.Add(key, new DataClass((int)tileEnum / 100, _pos, (int)tileEnum));
            ++key;
        }
        foreach(GameObject _player in players)
        {
            var playerSprite = _player.GetComponent<SpriteRenderer>().sprite;
            Vector3 _pos = _player.transform.position;
            tileEnum = TileType.walk_Sheet_2_0;
            infos.Add(key, new DataClass((int)tileEnum / 100, _pos, (int)tileEnum));
            ++key;
        }
        if(backGround != null) {
            var backGroundName = backGround.GetComponent<SpriteRenderer>().sprite.name;
            tileEnum = Enum.Parse(typeof(TileType), backGroundName);
            infos.Add(key, new DataClass((int)tileEnum / 100, new Vector3(0, 0, 0), (int)tileEnum));
            ++key;
        }
        infos.Add(key, new DataClass((int)TileType.MapSize / 100, _size["MIN"], (int)TileType.minSize));
        ++key;
        infos.Add(key, new DataClass((int)TileType.MapSize / 100, _size["MAX"], (int)TileType.maxSize));
    }

    public void Save(bool _forFile) {
        
        infos = new Dictionary<int, DataClass>();

        GetInfos();

        string toJson = JsonUtility.ToJson(new Serialization<int, DataClass>(infos));

        SaveDataClass _forSend = new SaveDataClass();
        _forSend.map_info = toJson;
        _forSend.map_tag = map_tag;
        _forSend.map_maker = "userId";

        string _forSendJson = JsonUtility.ToJson(_forSend);

        if(_forFile) {
            filePath = Application.streamingAssetsPath + "/";
            int num = 0;
            fullFilePath = filePath + fileName;

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            while(File.Exists(fullFilePath + ".json")) {
                fullFilePath = fullFilePath + "_" + num;
            }
            
            File.WriteAllText(fullFilePath + ".json", toJson);
            Debug.Log("save done");
        } else {
            StartCoroutine(MapSaveProcess(_result => {
                if(_result)
                {Debug.Log("SAVE");}
            }));

            IEnumerator MapSaveProcess(Action<bool> ResultHandler)
            {
                using (UnityWebRequest request = UnityWebRequest.Put(apiUrl + "api/map/apply", _forSendJson))
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
        //
    }
}