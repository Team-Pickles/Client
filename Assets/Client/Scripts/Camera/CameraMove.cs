using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject _player;
    private Vector3 _offset;

    private bool isShaking;
    public AnimationCurve curve;
    private float duration = 0.1f;
    private float elapsedTime = 0.0f;
    public bool IsShaking
    {
        get { return isShaking; }
        set { isShaking = value; }
    }
    void Start()
    {
        _player = GameObject.Find("Player");
        _offset = new Vector3(0.0f, -1.0f, 0.0f);
    }
    void Update()
    {
        if (isShaking)
        {
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = _player.transform.position + _offset + Random.insideUnitSphere * strength;

            elapsedTime += Time.deltaTime;
            if (elapsedTime > duration)
            {
                isShaking = false;
                elapsedTime = 0.0f;
            }
        }
        else
        {
            transform.position = _player.transform.position + _offset;
        }
    }
}
