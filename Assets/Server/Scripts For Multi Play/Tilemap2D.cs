using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tilemap2D : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private TMP_InputField inputWidth;

    [SerializeField]
    private TMP_InputField inputHeight;

    private MapData mapData;

    public int width { private set; get; } = 10;
    public int height { private set; get; } = 10;


    public List<Tile> tileList { private set; get; }
    private void Awake()
    {
        inputWidth.text = width.ToString();
        inputHeight.text = height.ToString();

        mapData = new MapData();
        tileList = new List<Tile>();
    }

    public void GenerateTilemap()
    {
        int _width, _height;

        int.TryParse(inputWidth.text, out _width);
        int.TryParse(inputHeight.text, out _height);

        width = _width;
        height = _height;

        for (int y=0; y<height; y++)
        {
            for (int x=0; x<width; x++)
            {
                Vector3 position = new Vector3((-width * 0.5f + 0.5f) + x, (height * 0.5f - 0.5f) - y, 0);

                SpawnTile(TileType.Empty, position);
            }
        }

        mapData.mapSize.x = width;
        mapData.mapSize.y = height;
        mapData.mapData = new int[tileList.Count];
    }

    private void SpawnTile(TileType _tileType, Vector3 _position)
    {
        GameObject clone = Instantiate(tilePrefab, _position, Quaternion.identity);

        clone.name = "Tile";
        clone.transform.SetParent(transform);

        Tile _tile = clone.GetComponent<Tile>();
        _tile.Setup(_tileType);

        tileList.Add(_tile);
    }

    public MapData GetMapData()
    {
        for (int i=0; i<tileList.Count; i++)
        {
            if (tileList[i].TileType != TileType.Player)
            {
                mapData.mapData[i] = (int)tileList[i].TileType;
            }
            else
            {
                mapData.mapData[i] = (int)TileType.Empty;

                int _x = (int)tileList[i].transform.position.x;
                int _y = (int)tileList[i].transform.position.y;
                mapData.playerPosition = new Vector2Int(_x, _y);
            }
        }
        return mapData;
    }

}
