using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1BoomDummy : MonoBehaviour
{
    private Vector2 _origin;
    private float _lifeTime;
    private float _time = 0.0f;
    public void Init(Vector2 origin, float lifeTime)
    {
        _origin = origin;
        _lifeTime = lifeTime;
        GetComponent<Rigidbody2D>().AddForce(((Vector2)transform.position - _origin).normalized * 130.0f);
        GetComponent<Rigidbody2D>().angularVelocity = 150.0f;
    }
    private void Update()
    {
        if (_time >= _lifeTime)
        {
            Destroy(gameObject);
        }
        _time += Time.deltaTime;
    }
}
