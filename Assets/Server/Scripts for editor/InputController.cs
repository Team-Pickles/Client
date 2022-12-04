using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

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
    private Sprite EmptySprite;

    [SerializeField]
    private Tilemap tileMap;

    private TileType currentType = TileType.Empty;
    public GameObject savePanel;

    [SerializeField]
    private CameraController cameraController;
    private Vector2 previousMousePosition;
    private Vector2 currentMousePosition;

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
        if (Input.GetMouseButton(1) && currentType != TileType.Empty)
        {
            SetTileType(0);
            return;
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
                        changeTile(_tilePose);

                if (isNull != null && currentType == TileType.Empty)
                    changeTile(_tilePose);
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

        currentType = (TileType)_tileType;
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        Image image = clickObject.GetComponent<Image>();
        Debug.Log($"{image}");
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
            tileMap.SetTile(_tilePose, itemBase[(int)(currentType - 1) - (int)TileType.Item]);
        }


        else if ((int)currentType < (int)TileType.Player)
        {
            tileMap.SetTile(_tilePose, enemyBase[(int)(currentType-1) - (int)TileType.Enemy]);
        }

        else if ((int)currentType >= (int)TileType.Player)
        {
            tileMap.SetTile(_tilePose, playerBase[(int)(currentType - 1) - (int)TileType.Player]);
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

}
