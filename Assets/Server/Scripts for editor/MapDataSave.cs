using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

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

    public void Save(InputField _mapName)
    {
        fileName = _mapName.text;
        filePath = Application.streamingAssetsPath + "/";
        fullFilePath = filePath + fileName;
        infos = new Dictionary<int, DataClass>();

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


    public void Share(GameObject _infoObject)
    {
        var _mapName = _infoObject.transform.GetChild(0).GetComponent<InputField>();
        Save(_mapName);
        var map_info = File.ReadAllText(fullFilePath + ".json");
        var map_tag = _infoObject.transform.GetChild(1).GetComponent<InputField>().text;
        var map_difficulty = _infoObject.transform.GetChild(2).GetComponent<InputField>().text;

        SaveDataClass _forSend = new SaveDataClass();
        _forSend.map_info = map_info;
        _forSend.map_tag = map_tag;
        _forSend.map_difficulty = Int32.Parse(map_difficulty);
        _forSend.map_maker = UserDataManager.instance.username;

        string _forSendJson = JsonUtility.ToJson(_forSend);

        StartCoroutine(MapShareProcess(_result =>
        {
            if (_result)
            { Debug.Log("SAVE"); }
        }));


        System.Collections.IEnumerator MapShareProcess(Action<bool> ResultHandler)
        {
            using (UnityWebRequest request = UnityWebRequest.Put(UserDataManager.instance.apiUrl + "api/map/apply", _forSendJson))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(_forSendJson);
                request.SetRequestHeader("Content-Type", "application/json");

                request.uploadHandler.Dispose();

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
}
