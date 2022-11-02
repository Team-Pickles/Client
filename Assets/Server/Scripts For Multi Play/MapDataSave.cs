using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class MapDataSave : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField inputFileName;

    [SerializeField]
    private Tilemap2D tilemap2D;

    private void Awake()
    {
        inputFileName.text = "NoName.json";
    }

    public void Save()
    {
        MapData _mapData = tilemap2D.GetMapData();
        string _fileName = inputFileName.text;

        if ( _fileName.Contains(".json") == false)
        {
            _fileName += ".json";
        }

        _fileName = Path.Combine("Mapdata/", _fileName);

        string _toJson = JsonUtility.ToJson(_mapData);

        File.WriteAllText(_fileName, _toJson);
    }
}
