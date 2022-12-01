using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class Tilemap2D : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private InputField inputWidth;

    [SerializeField]
    private InputField inputHeight;

    [SerializeField]
    private TileBase[] _tileBase;

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

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int position = new Vector3Int((int)(-width * 0.5f) + x, (int)(height * 0.5f) - y, 0);
                Debug.Log(position);
                SpawnTile(TileType.Empty, position);
            }
        }

        //타일맵 사이즈를 현재 소환된 타일맵의 경계의 크기 전환
        tilemap.CompressBounds();

        //tilemap.GetComponent<BoxCollider>().center = new Vector3((int)tilemap.cellBounds.center.x, (int)tilemap.cellBounds.center.y, 0) ;
        Debug.Log($"{tilemap.cellBounds.xMin},{tilemap.cellBounds.xMax},{tilemap.cellBounds.center.x}");
        tilemap.GetComponent<BoxCollider>().center = new Vector3(tilemap.cellBounds.center.x, tilemap.cellBounds.center.y, 0);
        tilemap.GetComponent<BoxCollider>().size = new Vector3(width*1.5f, height*1.5f, 0);
        
        mapData.mapSize.x = width;
        mapData.mapSize.y = height;
        mapData.mapData = new int[tileList.Count];
    }


    private void SpawnTile(TileType _tileType, Vector3Int _position)
    {
        tilemap.GetComponent<Tilemap>().SetTile(_position, _tileBase[0]);

    }


    public MapData GetMapData()
    {
        for (int i = 0; i < tileList.Count; i++)
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
