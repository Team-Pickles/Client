using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject _player;
    private Vector3 _offset;
    void Start()
    {
        _offset = new Vector3(0.0f, -1.0f, 0.0f);
    }
    void Update()
    {
        transform.position = _player.transform.position + _offset;
    }
}
