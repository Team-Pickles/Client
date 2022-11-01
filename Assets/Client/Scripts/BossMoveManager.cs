using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { IdleState = 0, MoveToPlayer = 1, Jumping = 2, GroundPound = 3, Attacking = 4}

public class BossMoveManager : MonoBehaviour
{
    private GameObject _grid;
    private MapManager _mm;
    private int _hp = 5;
    private int _bounceCount = 2;
    private Vector3Int _bossPosition;
    private Vector3 _aimPosition;
    private BossState _bossState = BossState.IdleState;
    private bool _onGround = false, _first = true, _isDead = false, _movement = true;
    private GameObject _trashPrefab;
    public bool OnGround
    {
        get { return _onGround; }
        set { _onGround = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        _grid = GameObject.Find("Grid");
        _mm = _grid.GetComponent<MapManager>();
        _bossPosition = _mm.GetTopLeftBasePosition(transform.position);
        _trashPrefab = (GameObject)Resources.Load("Prefabs/trash", typeof(GameObject));
    }
    public void ChangeState(BossState newState)
    {
        StopCoroutine(_bossState.ToString());
        _bossState = newState;
        StartCoroutine(_bossState.ToString());
    }
    private IEnumerator Movement()
    {
        float timeRemain = _hp;
        yield return new WaitForSeconds(timeRemain);
        _movement = false;
        yield return null;
    }
    private IEnumerator IdleState()
    {
        while(true)
        {
            if (_onGround)
            {
                yield return new WaitForSeconds(1.0f);
                ChangeState(BossState.MoveToPlayer);
            }
            yield return null;
        }
    }
    
    private IEnumerator MoveToPlayer()
    {
        Debug.Log("Boss::MoveToPlayer");
        _movement = true;
        while (true)
        {
            _bossPosition = _mm.GetTopLeftBasePosition(transform.position);
            if (_mm.PlayerPosition.x < _bossPosition.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (_mm.PlayerPosition.x > _bossPosition.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(2.0f, GetComponent<Rigidbody2D>().velocity.y);
            }
            StartCoroutine("Movement");
            if (!_movement)
            {
                StopCoroutine("Movement");
                ChangeState(BossState.Jumping);
            }
            yield return null;
        }
    }
    
    private IEnumerator Jumping()
    {
        _bounceCount = _hp / 2;
        while(true)
        {
            _bossPosition = _mm.GetTopLeftBasePosition(transform.position);
            if (_mm.PlayerPosition.x < _bossPosition.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (_mm.PlayerPosition.x > _bossPosition.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(2.0f, GetComponent<Rigidbody2D>().velocity.y);
            }
            if (_onGround)
            {
                if(_bounceCount > 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.y, 5.0f);
                    _bounceCount--;
                    _onGround = false;
                    Debug.Log(_bounceCount + " left");
                }
            }
            if (_onGround && _bounceCount == 0)
                ChangeState(BossState.GroundPound);
            yield return null;
        }
    }
    
    private IEnumerator GroundPound()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        _bossPosition = _mm.GetTopLeftBasePosition(transform.position);
        _aimPosition = new Vector3(transform.position.x - _bossPosition.x + _mm.PlayerPosition.x, transform.position.y - _bossPosition.y + _mm.PlayerPosition.y + 4);
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            transform.position = _aimPosition;
            yield return new WaitForSeconds(3.0f);
            ChangeState(BossState.Attacking);
            yield return null;
        }
    }
    
    private IEnumerator Attacking()
    {
        while(true)
        {
            GameObject trashLeft, trashRight;
            trashLeft = Object.Instantiate(_trashPrefab, new Vector3(transform.position.x - transform.localScale.x / 2.0f * 1.2f, transform.position.y + transform.localScale.y / 2.0f * 1.1f, 0), new Quaternion());
            trashRight = Object.Instantiate(_trashPrefab, new Vector3(transform.position.x + transform.localScale.x / 2.0f * 1.2f, transform.position.y + transform.localScale.y / 2.0f * 1.1f, 0), new Quaternion());
            trashLeft.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-200.0f, 300.0f));
            trashLeft.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(200.0f, 300.0f));
            yield return new WaitForSeconds(3.0f);
            ChangeState(BossState.IdleState);
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch (collision.transform.tag)
        {
            case "bullet":
                {
                    Destroy(collision.gameObject);
                    _hp--;
                    if(_hp == 0)
                    {
                        _isDead = true;
                        Destroy(gameObject);
                    }
                    break;
                }
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.name == "Player" && _isDead == false)
        {
            collision.transform.GetComponent<PlayerMoveManager>().OnDamagedAction();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_first)
        {
            ChangeState(BossState.IdleState);
            _first = false;
        }
    }
}
