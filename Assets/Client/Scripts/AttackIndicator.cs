using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    SpriteRenderer _sr;
    float _radius;
    float _thickness;
    GameObject dummy;
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _radius = _sr.material.GetFloat("_Radius");
        _thickness = _sr.material.GetFloat("_Thickness");

        dummy = Resources.Load("Prefabs/Boss1/Boss1BoomDummy") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _radius -= Time.deltaTime / 2.0f;
        _sr.material.SetFloat("_Radius", _radius);
        if (_radius <= _thickness)
        {
            Boss1Attack();
            Destroy(gameObject);
        }
    }

    public void Boss1Attack()
    {
        int cnt = 5;
        float offset = Random.Range(0.0f, Mathf.PI);
        for (int i = 0; i < cnt; i++)
        {
            float xPos = transform.position.x + 0.1f * Mathf.Cos(2.0f * Mathf.PI / cnt * i + offset);
            float yPos = transform.position.y + 0.1f * Mathf.Sin(2.0f * Mathf.PI / cnt * i + offset);
            GameObject obj = Instantiate(dummy, new Vector2(xPos, yPos), new Quaternion());
            obj.GetComponent<Boss1BoomDummy>().Init(transform.position, 10.0f);
        }
    }
}
