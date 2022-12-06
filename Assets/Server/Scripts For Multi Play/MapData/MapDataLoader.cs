using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class MapDataLoader : MonoBehaviour
{
    [SerializeField] Tilemap MapGrid;
    public GameObject background;
    public TileBase[] TileBases;
    public Sprite[] Backgrounds;

    public static MapDataLoader instance;

    private void Awake()
    {
        //Singleton ����
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists,destroying object!");
            Destroy(this);
        }
    }

    public void Load(string _fromJson) {
        Refresh();

        Dictionary<int, DataClass> loaded = JsonUtility.FromJson<Serialization<int, DataClass>>(_fromJson).ToDictionary();
        int[] _deathZone = new int[4];
        foreach(DataClass data in loaded.Values) {
            if(data.GetInfoType() == (int)TileTypes.Empty / 100) {
                int tileType = data.GetAdditionalInfo();
                try {
                    Vector3 _pos = data.GetPos();
                    Vector3Int _intPos = new Vector3Int((int)_pos.x, (int)_pos.y, (int)_pos.z);
                    MapGrid.SetTile(_intPos, TileBases[tileType - 1]);
                } catch {
                    Debug.Log(tileType);
                }
            }
            else if (data.GetInfoType() == (int)TileTypes.BackGround / 100)
            {
                int backgroundType = data.GetAdditionalInfo();
                try {
                    background.GetComponent<SpriteRenderer>().sprite = Backgrounds[backgroundType - 1];
                } catch {
                    Debug.Log(backgroundType);
                }
            }
        }
        Debug.Log("load done");
    }

    public void Refresh() {
        MapGrid.ClearAllTiles();

        // int itemCnt = ItemGroup.transform.childCount;
        // int enemyCnt = EnemyGroup.transform.childCount;
        // int playerCnt = PlayerGroup.transform.childCount;

        // List<GameObject> forDestroy = new List<GameObject>();
        // for (int i = 0; i < itemCnt; ++i) {
        //     forDestroy.Add(ItemGroup.transform.GetChild(i).gameObject);
        // }
        // for (int i = 0; i < enemyCnt; ++i) {
        //     forDestroy.Add(EnemyGroup.transform.GetChild(i).gameObject);
        // }
        // for (int i = 0; i < playerCnt; ++i) {
        //     forDestroy.Add(PlayerGroup.transform.GetChild(i).gameObject);
        // }

        // foreach (GameObject obj in forDestroy) {
        //     DestroyImmediate(obj);
        // }
    }
}
