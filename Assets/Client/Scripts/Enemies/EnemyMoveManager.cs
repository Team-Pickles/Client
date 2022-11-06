using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveManager : MonoBehaviour
{
    public MapManager mm;

    private Vector3Int _enemyPosition;
    private const float _threshold = 1.0f;
    private float _currentHold = 0.0f;
    private bool _onGround = false;
    public bool OnGround
    {
        get { return _onGround; }
        set { _onGround = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        mm = GameObject.Find("Grid").GetComponent<MapManager>();
        _enemyPosition = mm.GetTopLeftBasePosition(transform.position);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "bullet":
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    
    void Update()
    {
        _enemyPosition = mm.GetTopLeftBasePosition(transform.position);

        if ((_enemyPosition.x - mm.PlayerPosition.x) * (_enemyPosition.x - mm.PlayerPosition.x)
            + (_enemyPosition.y - mm.PlayerPosition.y) * (_enemyPosition.y - mm.PlayerPosition.y) <= 49)
        {
            if (_onGround && _enemyPosition.y > mm.PlayerPosition.y)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 5.0f);
            }

            if (GetComponent<Rigidbody2D>().velocity.x > 0.3f)
            {
                if (mm.PlayerPosition.x < _enemyPosition.x)
                {
                    _currentHold += 5.0f * Time.deltaTime;
                }
                else
                {
                    _currentHold = 0.0f;
                }
                if (_currentHold >= _threshold && _onGround)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, GetComponent<Rigidbody2D>().velocity.y);
                    _currentHold = 0.0f;
                }
            }
            else if (GetComponent<Rigidbody2D>().velocity.x < -0.3f)
            {
                if (mm.PlayerPosition.x > _enemyPosition.x)
                {
                    _currentHold += 5.0f * Time.deltaTime;
                }
                else
                {
                    _currentHold = 0.0f;
                }
                if (_currentHold >= _threshold && _onGround)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(2.0f, GetComponent<Rigidbody2D>().velocity.y);
                    _currentHold = 0.0f;
                }
            }
            else
            {
                if (mm.PlayerPosition.x < _enemyPosition.x)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, GetComponent<Rigidbody2D>().velocity.y);
                }
                else if (mm.PlayerPosition.x > _enemyPosition.x)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(2.0f, GetComponent<Rigidbody2D>().velocity.y);
                }
                if (_onGround)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 5.0f);
                }
            }
        }
        else
        {
            if (GetComponent<Rigidbody2D>().velocity == new Vector2(0,0))
            {
                if (Random.Range(1,10) <= 5)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, GetComponent<Rigidbody2D>().velocity.y);
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(2.0f, GetComponent<Rigidbody2D>().velocity.y);
                }
            }
            //GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, GetComponent<Rigidbody2D>().velocity.y);
        }

        /*
        if (mm.PlayerPosition.x < _enemyPosition.x)
        {
            //Debug.Log("왼쪽으로 가야합니다.");
            GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, 0.0f);
        }
        else if (mm.PlayerPosition.x > _enemyPosition.x)
        {
            //Debug.Log("오른쪽으로 가야합니다.");
            GetComponent<Rigidbody2D>().velocity = new Vector2(2.0f, 0.0f);
        }
        else
        {
            //Debug.Log("안움직여도 됩니다.");
        }
        */
    }
}
