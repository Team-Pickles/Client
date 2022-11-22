using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerStateFlags
{
    Normal = 1 << 0,
    Stun = 1 << 1,
    Damaged = 1 << 2
}

public class PlayerMoveManager : MonoBehaviour
{
    private bool _onGround = false;
    private bool _onRope = false;
    private bool _isHanging = false;
    private float _xAxisDrag = 0.005f;
    private float _hPoint = 0, _vPoint = 0;
    private const float _hSpeed = 4.0f, _vSpeed = 5.0f;
    private bool _leftPressed = false, _rightPressed = false;
    private bool _runState = false;
    private bool _flip = false;
    public PlayerStateFlags _state = PlayerStateFlags.Normal;
    private int _hp = 3;
    private int _bulletCount = 0;
    private int _grenadeCount = 0;
    private int _glassBottleCount = 0;

    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public GameObject glassbottlePrefab;
    public Animator animator;

    public GameObject vacuumCleaner;
    private Animator vacuumAnimator;
    private Vector3 recentRopePosition;
    private int ropeCollisionCount = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "rope":
            {
                ropeCollisionCount++;
                _onRope = true;
                recentRopePosition = collision.transform.position;
                break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "rope":
            {
                ropeCollisionCount--;
                if (ropeCollisionCount == 0)
                {
                    _onRope = false;
                    _isHanging = false;
                    GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                }
                break;
            }
        }
    }
    public void ResetVariable()
    {
        _onGround = false;
        _hPoint = 0;
        _vPoint = 0;
        _leftPressed = false;
        _rightPressed = false;
        _runState = false;
        _flip = false;
        vacuumCleaner.GetComponent<SpriteRenderer>().flipX = false;
        _state = PlayerStateFlags.Normal;
        _hp = 3;
        _bulletCount = 0;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public bool OnGround
    {
        get { return _onGround; }
        set { _onGround = value; }
    }
    public int BulletCount
    {
        get { return _bulletCount; }
    }
    public int Hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public int GrenadeCount
    {
        get { return _grenadeCount; }
    }
    public int GlassBottleCount
    {
        get { return _glassBottleCount; }
    }
    public void OnJumpSpringAction()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
        _vPoint = 1.5f;
    }
    public void OnJumpAction()
    {
        _vPoint = 1.2f;
    }
    public void SetPlayerStateFlags(PlayerStateFlags flag)
    {
        _state |= flag;
    }
    public void ResetPlayerStateFlags(PlayerStateFlags flag)
    {
        _state &= ~flag;
    }

    public void IncreaseBullet()
    {
        _bulletCount++;
    }
    public void DecreaseBullet()
    {
        _bulletCount--;
    }
    public void IncreaseGrenade(int amount)
    {
        _grenadeCount += amount;
        if (_grenadeCount >= 100)
            _grenadeCount = 99;
    }
    public void DecreaseGrenade()
    {
        _grenadeCount--;
    }
    public void IncreaseGlassBottle(int amount)
    {
        _glassBottleCount += amount;
        if (_glassBottleCount >= 100)
            _glassBottleCount = 99;
    }
    public void DecreaseGlassBottle()
    {
        _glassBottleCount--;
    }
    private IEnumerator Damaged()
    {
        if ((_state & PlayerStateFlags.Damaged) == 0)
        {
            _hp--;
            SetPlayerStateFlags(PlayerStateFlags.Damaged);
            SetPlayerStateFlags(PlayerStateFlags.Stun);
            _hPoint = -0.7f * (_flip==false ? 1 : -1);
            _vPoint = 0.5f;
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
            Debug.Log(_hp + " 남았습니다");
            for (int i=0;i<6;i++)
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
            _hPoint = 0.0f;
            // immortal state
            {
                ResetPlayerStateFlags(PlayerStateFlags.Stun);
                Color color = GetComponent<SpriteRenderer>().color;
                color.a = 0.7f;
                GetComponent<SpriteRenderer>().color = color;

                yield return new WaitForSeconds(0.7f);

                color.a = 1.0f;
                GetComponent<SpriteRenderer>().color = color;
                ResetPlayerStateFlags(PlayerStateFlags.Damaged);
            }
            yield break;
        }
    }
    private IEnumerator Explosion(float hValue, float vValue)
    {
        SetPlayerStateFlags(PlayerStateFlags.Stun);
        _hPoint = hValue;
        _vPoint = vValue;
        yield return new WaitForSeconds(2);
        ResetPlayerStateFlags(PlayerStateFlags.Stun);
        yield break;
    }
    public void OnDamagedAction()
    {
        StartCoroutine(Damaged());
    }
    public void OnExplosionAction(float hValue, float vValue)
    {
        StartCoroutine(Explosion(hValue, vValue));
    }

    private bool CanControl()
    {
        return (_state & PlayerStateFlags.Stun) == 0;
    }
    void Start()
    {
        vacuumAnimator = vacuumCleaner.GetComponent<Animator>();
        bulletPrefab = (GameObject)Resources.Load("Prefabs/bullet", typeof(GameObject));
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(_hPoint * _hSpeed, GetComponent<Rigidbody2D>().velocity.y + _vPoint * _vSpeed);
        _vPoint = 0.0f;
        GetComponent<SpriteRenderer>().flipX = _flip;
    }
    void Update()
    {
        // 점프
        if (_onGround && Input.GetKeyDown(KeyCode.D) && CanControl())
        {
            OnJumpAction();
            if (_isHanging)
                _isHanging = false;
        }
        // 위 (로프)
        if (CanControl())
        {
            // 매달리기 시작
            if (_onRope && Input.GetKeyDown(KeyCode.UpArrow))
            {
                _isHanging = true;
                transform.position = new Vector2(recentRopePosition.x, transform.position.y);
            }
            if (_isHanging)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    _vPoint = 1.2f;
                    _isHanging = false;
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    _vPoint = 0.8f;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _vPoint = -0.8f;
                }
                else
                {
                    _vPoint = 0.0f;
                }
            }
        }
        // 매달린 상태 체크
        if (_isHanging)
        {
            vacuumCleaner.SetActive(false);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            animator.SetBool("isHanging", true);
        }
        else
        {
            vacuumCleaner.SetActive(true);
            GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            animator.SetBool("isHanging", false);
        }
        // 좌우
        _leftPressed = Input.GetKey(KeyCode.LeftArrow);
        _rightPressed = Input.GetKey(KeyCode.RightArrow);
        _runState = Input.GetKey(KeyCode.X);
        float speed = 1.5f;
        if (CanControl())
        {
            speed += _runState == true ? 1 : 0;
            _hPoint = (_leftPressed == true ? -speed : 0) + (_rightPressed == true ? speed : 0);
            if (_isHanging)
                _hPoint = 0.0f;
            // animation change
            if (_hPoint != 0)
            {
                animator.SetBool("isWalking", true);
                vacuumAnimator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
                vacuumAnimator.SetBool("isWalking", false);
            }

            if (_hPoint > 0)
            {
                Vector3 vacuumPosition = vacuumCleaner.transform.localPosition;
                vacuumPosition.x = 0.35f;
                vacuumCleaner.transform.localPosition = vacuumPosition;
                vacuumCleaner.GetComponent<SpriteRenderer>().flipX = false;

                Vector2 offset = vacuumCleaner.GetComponent<BoxCollider2D>().offset;
                offset.x = -0.15f;
                vacuumCleaner.GetComponent<BoxCollider2D>().offset = offset;

                offset = GetComponent<BoxCollider2D>().offset;
                offset.x = 0.06f;
                GetComponent<BoxCollider2D>().offset = offset;
                _flip = false;
            }
            else if (_hPoint < 0)
            {
                Vector3 vacuumPosition = vacuumCleaner.transform.localPosition;
                vacuumPosition.x = -0.35f;
                vacuumCleaner.transform.localPosition = vacuumPosition;
                vacuumCleaner.GetComponent<SpriteRenderer>().flipX = true;

                Vector2 offset = vacuumCleaner.GetComponent<BoxCollider2D>().offset;
                offset.x = 0.15f;
                vacuumCleaner.GetComponent<BoxCollider2D>().offset = offset;

                offset = GetComponent<BoxCollider2D>().offset;
                offset.x = -0.06f;
                GetComponent<BoxCollider2D>().offset = offset;
                _flip = true;
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            vacuumAnimator.SetBool("isWalking", false);
            vacuumAnimator.SetBool("isConsuming", false);
        }
        _hPoint = _hPoint / (_xAxisDrag + 1.0f);
    }
}


/*
        switch (_state)
        {
            case PlayerStateFlags.Normal:
            {
                float speed = 1.0f;
                speed += _runState == true ? 1 : 0;
                _hPoint = (_leftPressed == true ? -speed : 0) + (_rightPressed == true ? speed : 0);
                break;
            }
            case PlayerStateFlags.Stun:
            {
                break;
            }
        }
        */