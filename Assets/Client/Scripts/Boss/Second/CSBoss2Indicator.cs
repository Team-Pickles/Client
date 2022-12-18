using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSBoss2Indicator : MonoBehaviour
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

        _dummy = Resources.Load("Prefabs/CSBoss2/Enemy-can") as GameObject;
        _trash = Resources.Load("Prefabs/Trash") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _radius -= Time.deltaTime / 2.0f;
        _sr.material.SetFloat("_Radius", _radius);
        if (_radius <= _thickness)
        {
            if (rand <= 0.2f)
                Trash();
            else
                Attack();
            Destroy(gameObject);
        }
    }
    private void Trash()
    {
        int cnt = 3;
        float offset = Random.Range(0.0f, Mathf.PI);
        for (int i = 0; i < cnt; i++)
        {
            float xPos = transform.position.x + 0.5f * Mathf.Cos(2.0f * Mathf.PI / cnt * i + offset);
            float yPos = transform.position.y + 0.5f * Mathf.Sin(2.0f * Mathf.PI / cnt * i + offset);
            Vector2 spawnPos = new Vector2(xPos, yPos);
            GameObject obj = Instantiate(_trash, spawnPos, new Quaternion());
            obj.GetComponent<Rigidbody2D>().AddForce((spawnPos - (Vector2)transform.position).normalized * 200.0f);
        }
    }
    private void Attack()
    {
        GameObject obj = Instantiate(_dummy, transform.position, new Quaternion());
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5000.0f));
        
    }
}
