using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Tilemap2D tilemap2D;
    [SerializeField]
    private Tilemap tilemap;
    private Camera mainCamera;

    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private float zoomSpeed = 1000;
    [SerializeField]
    private float minViewSize=5;
    private float maxViewSize = 15;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void SetupCamere()
    {
        int _width = tilemap2D.width;
        int _height = tilemap2D.height;

        //float _size = (_width > _height) ? _width * wDelta : _height * hdelta;
        transform.position = new Vector3(tilemap.cellBounds.center.x, tilemap.cellBounds.center.y,-1);
    }

    public void SetPosition(float _x, float _y)
    {
        transform.position += new Vector3(_x * moveSpeed * Time.deltaTime, _y * moveSpeed * Time.deltaTime, 0)  ;
    }

    public void SetOrthopfraphicSize(float _size)
    {
        if (_size== 0) return;

        mainCamera.orthographicSize += _size * zoomSpeed * Time.deltaTime;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minViewSize, maxViewSize);
    }

}
