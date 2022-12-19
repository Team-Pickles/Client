using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicatorInServer : MonoBehaviour
{
    SpriteRenderer _sr;
    float _radius;
    float _thickness;
    GameObject _dummy;
    GameObject _trash;
    float rand;
    Color32 _attackColor = new Color32(255, 66, 66, 255);
    Color _trashColor = new Color(0.2f, 0.8f, 0.1f, 1.0f);
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _radius = _sr.material.GetFloat("_Radius");
        _thickness = _sr.material.GetFloat("_Thickness");

        rand = Random.Range(0.0f, 1.0f);
        if (rand <= 0.2f)
            _sr.material.SetColor("_Color", _trashColor);
        else
            _sr.material.SetColor("_Color", _attackColor);

        _dummy = Resources.Load("Prefabs/Boss1/Boss1BoomDummy") as GameObject;
        _trash = Resources.Load("Prefabs/Trash") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _radius -= Time.deltaTime / 2.0f;
        _sr.material.SetFloat("_Radius", _radius);
        if (_radius <= _thickness)
        {
            Destroy(gameObject);
        }
    }
}
