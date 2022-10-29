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
    private float _xAxisDrag = 0.005f;
    private float _hPoint = 0, _vPoint = 0;
    private const float _hSpeed = 4.0f, _vSpeed = 5.0f;
    private bool _leftPressed = false, _rightPressed = false;
    private bool _runState = false;
    private bool _flip = false;
    public PlayerStateFlags _state = PlayerStateFlags.Normal;
    private int _hp = 3;
    private int _bulletCount = 0;

    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public GameObject glassbottlePrefab;

    public void ResetVariable()
    {
        _onGround = false;
        _hPoint = 0;
        _vPoint = 0;
        _leftPressed = false;
        _rightPressed = false;
        _runState = false;
        _flip = false;
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
    public void OnJumpAction()
    {
        _vPoint = 1.0f;
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
            Debug.Log(_hp + " ���ҽ��ϴ�");
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
        // ��
        if (_onGround && Input.GetKeyDown(KeyCode.UpArrow) && CanControl())
            OnJumpAction();

        // �¿�
        _leftPressed = Input.GetKey(KeyCode.LeftArrow);
        _rightPressed = Input.GetKey(KeyCode.RightArrow);
        _runState = Input.GetKey(KeyCode.X);
        float speed = 1.0f;
        if (CanControl())
        {
            speed += _runState == true ? 1 : 0;
            _hPoint = (_leftPressed == true ? -speed : 0) + (_rightPressed == true ? speed : 0);
            if (_hPoint > 0)
                 _flip= false;
            else if (_hPoint < 0)
                _flip = true;
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