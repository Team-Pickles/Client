using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    [SerializeField]
    public Camera mainCamera;
    [SerializeField]
    private Button currnetButton;
    [SerializeField]
    private TileBase[] tileBase;
    [SerializeField]
    private TileBase[] itemBase;
    [SerializeField]
    private TileBase[] enemyBase;
    [SerializeField]
    public TileBase[] playerBase;
    [SerializeField]
    public Sprite[] backGroundBase;
    [SerializeField]
    public TileBase[] doorBase;
    [SerializeField]
    private Sprite EmptySprite;
    public NoticeUI _noticeUI;
    public GameObject AskQuitUi;

    [SerializeField]
    private Tilemap tileMap;

    private TileType currentType = TileType.Empty;
    public GameObject savePanel;

    [SerializeField]
    private CameraController cameraController;
    private Vector2 previousMousePosition;
    private Vector2 currentMousePosition;

    private int doorId = 1;
    private bool isIndoorSet = false;
    private Vector3Int prevIndoorPos;
    public static Dictionary<Vector3Int, int> doors = new Dictionary<Vector3Int, int>();

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
        if (Input.GetMouseButton(1) && currentType != TileType.Empty)
        {
            SetTileType(0);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(AskQuitUi.activeSelf)
                AskQuitUi.SetActive(false);
            else
                AskQuitUi.SetActive(true);
        }

        RaycastHit _hit;
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false) 
        {
            //카메라에서 현재 마우스 위치로 ray 발사
            Ray _ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(_ray.origin,_ray.direction, Color.red,10f);
            if (Physics.Raycast(_ray, out _hit, 10))
            {
                //ray와 충돌한 오브젝트의 Tile 컴포넌트 정보를 저장
                //타일이 없으면 null
                Tilemap _tileMap = _hit.transform.GetComponent<Tilemap>();
                var _tilePose = _tileMap.WorldToCell(_hit.point);
                var _tile = _tileMap.GetTile(_tilePose);
                var isNull = _tileMap.GetSprite(_tilePose);

                if (isNull != null)
                    if (_tileMap.GetSprite(_tilePose).name == "Empty")
                    {
                        if ((int)currentType == (int)TileType.barricade && _tileMap.GetSprite(_tilePose + new Vector3Int(0, 1, 0)).name != "Empty")
                            _noticeUI.AlertBox("barricade 는 세로로 2칸을 필요로 합니다!");
                        else
                            changeTile(_tilePose);
                    }
                        

                if (isNull != null && currentType == TileType.Empty)
                {
                    if(_tileMap.GetSprite(_tilePose).name.Contains("door")) {
                        int doorNum = -1;
                        doors.TryGetValue(_tilePose, out doorNum);
                        if(doorNum != -1 && doors.Count > 0) {
                            doorNum = doors[_tilePose]/ 1000;
                            changeTile(_tilePose);
                            doors.Remove(_tilePose);
                            foreach(KeyValuePair<Vector3Int, int> _door in doors)
                            {
                                if(_door.Value/1000 == doorNum)
                                {
                                    changeTile(_door.Key);
                                    doors.Remove(_door.Key);
                                    break;
                                }
                            }
                        }
                    }
                    else if (_tileMap.GetSprite(_tilePose).name.Contains("barricade"))
                    {
                        changeTile(_tilePose + new Vector3Int(0,1,0));
                        changeTile(_tilePose);
                    }
                    else
                    {
                        changeTile(_tilePose);
                    }
                }
            }
        }
    }
    public void SetTileType(int _tileType)
    {
        if (_tileType == 0)
        {
            currentType = (TileType)_tileType;
            currnetButton.GetComponent<Image>().sprite = EmptySprite; 
            return;
        }
        
        if(isIndoorSet && _tileType != (int)TileType.outdoor)
        {
            tileMap.SetTile(prevIndoorPos, tileBase[(int)TileType.Empty]);
            isIndoorSet = false;
        }
        if(!isIndoorSet && _tileType == (int)TileType.outdoor)
        {
            return;
        }
        currentType = (TileType)_tileType;
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        Image image = clickObject.GetComponent<Image>();
        currnetButton.GetComponent<Image>().sprite =  image.sprite;
    }

    public void changeTile(Vector3Int _tilePose)
    {
        if ((int)currentType < (int)TileType.Item)
        {
            tileMap.SetTile(_tilePose, tileBase[(int)currentType]);
        }

        // 100 <= , <200
        else if ((int)currentType < (int)TileType.Enemy)
        {
            if ((int)currentType == (int)TileType.barricade)
                tileMap.SetTile(_tilePose+new Vector3Int(0,1,0), null);
            
            tileMap.SetTile(_tilePose, itemBase[(int)(currentType - 1) - (int)TileType.Item]);
        }


        else if ((int)currentType < (int)TileType.Player)
        {
            tileMap.SetTile(_tilePose, enemyBase[(int)(currentType-1) - (int)TileType.Enemy]);
        }

        else if ((int)currentType < (int)TileType.BackGround)
        {
            tileMap.SetTile(_tilePose, playerBase[(int)(currentType - 1) - (int)TileType.Player]);
        }
        else if((int)currentType == (int)TileType.indoor && !isIndoorSet)
        {
            prevIndoorPos = _tilePose;
            tileMap.SetTile(_tilePose, doorBase[(int)(currentType - 1) - (int)TileType.door]);
            isIndoorSet = true;
        }
        else if((int)currentType == (int)TileType.outdoor && isIndoorSet)
        {
            doors.Add(prevIndoorPos, 1000 * doorId + (int)TileType.indoor);
            doors.Add(_tilePose, 1000 * doorId + (int)TileType.outdoor);
            tileMap.SetTile(_tilePose, doorBase[(int)(currentType - 1) - (int)TileType.door]);
            ++doorId;
            isIndoorSet = false;
        }
    }

    public void changeBackGround(int _type)
    {
        cameraController.GetComponentInChildren<SpriteRenderer>().sprite = backGroundBase[_type];
    }

    public void UpdateCamera()
    {
        float _x = Input.GetAxisRaw("Horizontal");
        float _y = Input.GetAxisRaw("Vertical");
        if(!savePanel.activeInHierarchy)
            cameraController.SetPosition(_x, _y);

        if (Input.GetMouseButtonDown(2))
        {
            currentMousePosition = previousMousePosition = Input.mousePosition;
        }

        else if (Input.GetMouseButton(2))
        {
            currentMousePosition = Input.mousePosition;
            if (previousMousePosition != currentMousePosition)
            {
                Vector2 _move = (previousMousePosition - currentMousePosition) * 0.5f;
                cameraController.SetPosition(_move.x, _move.y);
            }
        }
        previousMousePosition = currentMousePosition;

        

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            float _distance = Input.GetAxisRaw("Mouse ScrollWheel");
            cameraController.SetOrthopfraphicSize(-_distance);
        }

    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
