using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MapDataSaverWOTilemap : MonoBehaviour
{

    [SerializeField]
    private Tilemap2D tilemap2D;
    private string fullFilePath;

    [SerializeField] private string filePath = "MapData/";
    [SerializeField] string fileName = "MyMap";

    public void Save()
    {
        MapData _mapData = tilemap2D.GetMapData();

        fullFilePath = filePath + fileName;
        string _toJSon = JsonUtility.ToJson(_mapData);
        int num = 0;

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        while (File.Exists(fullFilePath + ".json"))
        {
            fullFilePath = fullFilePath + "_" + num;
        }
        File.WriteAllText(fullFilePath + ".json", _toJSon);

        Debug.Log("save done");
    }
}
