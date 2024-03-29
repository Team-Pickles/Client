using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum BossState { IdleState = 0, MoveToPlayer = 1, Jumping = 2, GroundPound = 3, Attacking = 4}

public class BossMoveManager : MonoBehaviour
{
    public GameObject trashPrefab;

    private GameObject _grid, _bossEnd;
    private MapManager _mm;
    private int _hp = 5;
    private int _bounceCount = 2;
    private Vector3Int _bossPosition;
    private Vector3 _aimPosition;
    private BossState _bossState = BossState.IdleState;
    private BossHp _bossHp;
    private bool _onGround = false, _first = true, _movement = true, _immortal = true, _playerInSight = false;

    public string nextLevel;

    public bool OnGround
    {
        get { return _onGround; }
        set { _onGround = value; }
    }
    public bool PlayerInSight
    {
        get { return _playerInSight; }
        set { _playerInSight = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        _grid = GameObject.Find("Grid");
        _bossEnd = GameObject.Find("EndLevelUI");
        _mm = _grid.GetComponent<MapManager>();
        _bossPosition = _mm.GetTopLeftBasePosition(transform.position);
        _bossHp = GetComponent<BossHp>();
        _bossHp.Hp = _hp;
    }
    public void ChangeState(BossState newState)
    {
        StopCoroutine(_bossState.ToString());
        _bossState = newState;
        StartCoroutine(_bossState.ToString());
    }
    private IEnumerator Damaged()
    {
        _immortal = true;
        _hp--;
        Debug.Log("Boss hp: "+_hp + " left");
        for (int i = 0; i < 6; i++)
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.7f;
            if (color == new Color(1.0f, 1.0f, 1.0f, 0.7f))
            {
                color.g = 0.3f;
                color.b = 0.3f;
            }
            else
            {
                color.g = 1.0f;
                color.b = 1.0f;
            }
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.1f);
        }
        // immortal state
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.7f;
            GetComponent<SpriteRenderer>().color = color;

            yield return new WaitForSeconds(0.7f);

            color.r = 1.0f;
            color.g = 1.0f;
            color.b = 0.0f;
            color.a = 1.0f;
            GetComponent<SpriteRenderer>().color = color;
        }
        _immortal = false;
        yield break;
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
        Debug.Log("Boss::IdleState");
        while (true)
        {
            if (_onGround && _playerInSight)
            {
                _immortal = false;
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
        Debug.Log("Boss::Jumping");
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
                    Debug.Log("bounce "+_bounceCount + " left");
                }
            }
            if (_onGround && _bounceCount == 0)
                ChangeState(BossState.GroundPound);
            yield return null;
        }
    }
    
    private IEnumerator GroundPound()
    {
        Debug.Log("Boss::GroundPound");
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        _bossPosition = _mm.GetTopLeftBasePosition(transform.position);
        _aimPosition = new Vector3((transform.position.x - _bossPosition.x + _mm.PlayerPosition.x), (transform.position.y - _bossPosition.y + _mm.PlayerPosition.y + 4));
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            transform.position = _aimPosition;
            Debug.Log("Boss::Teleport");
            yield return new WaitForSeconds(3.0f);
            ChangeState(BossState.Attacking);
            yield return null;
        }
    }
    
    private IEnumerator Attacking()
    {
        Debug.Log("Boss::Attacking");
        while (true)
        {
            GameObject trashLeft, trashRight;
            trashLeft = Object.Instantiate(trashPrefab, new Vector3(transform.position.x - transform.localScale.x / 2.0f * 1.4f, transform.position.y + transform.localScale.y / 2.0f * 1.1f, 0), new Quaternion());
            trashRight = Object.Instantiate(trashPrefab, new Vector3(transform.position.x + transform.localScale.x / 2.0f * 1.4f, transform.position.y + transform.localScale.y / 2.0f * 1.1f, 0), new Quaternion());
            trashLeft.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-200.0f, 300.0f));
            trashRight.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(200.0f, 300.0f));
            yield return new WaitForSeconds(3.0f);
            ChangeState(BossState.IdleState);
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_immortal && collision.name == "Bullet(Clone)")
        {
            Destroy(collision.gameObject);
            StartCoroutine(Damaged());
            if (_hp == 0)
                Destroy(gameObject);
        }
    }
    private IEnumerator Clear()
    {
        _bossEnd.GetComponent<EndLevelUI>().EndLevel();
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(nextLevel);
        GameObject.Find("Player").transform.position = new Vector3(-5.0f, -2.0f, 0.0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
                {
                    collision.transform.GetComponent<PlayerMoveManager>().OnDamagedAction();
                    break;
                }
            case "bullet":
                {
                    StartCoroutine(Damaged());
                    if (_hp == 0)
                    {
                        StartCoroutine(Clear());

                    }
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _bossHp.Hp = _hp;
        if(_first)
        {
            ChangeState(BossState.IdleState);
            _first = false;
        }
    }
}
