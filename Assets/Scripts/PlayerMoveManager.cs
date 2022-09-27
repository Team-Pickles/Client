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
    private int _hp = 3;
    private bool _onGround = false;
    private float _xAxisDrag = 0.005f;
    private float _hPoint = 0, _vPoint = 0;
    private const float _hSpeed = 4.0f, _vSpeed = 5.0f;
    private bool _leftPressed = false, _rightPressed = false;

    public PlayerStateFlags _state = PlayerStateFlags.Normal;
    private int _bulletCount;

    public GameObject bulletPrefab;
    
    public int Hp
    {
        get { return _hp; }
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
        //Debug.Log("DataStorage::BulletCount Increased");
    }
    public void DecreaseBullet()
    {
        _bulletCount--;
    }
    private IEnumerator Damaged()
    {
        if((_state & PlayerStateFlags.Damaged) == 0)
        {
            _hp--;
            SetPlayerStateFlags(PlayerStateFlags.Damaged);
            Debug.Log(_hp + " 남았습니다");
            yield return new WaitForSeconds(3);
            ResetPlayerStateFlags(PlayerStateFlags.Damaged);
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
    public void OnExplosionAction(float hValue, float vValue)
    {
        StartCoroutine(Explosion(hValue, vValue));
    }

    public void OnDamagedAction()
    {
        StartCoroutine(Damaged());
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
    }
    void Update()
    {
        // 위
        if (_onGround && Input.GetKeyDown(KeyCode.UpArrow) && CanControl())
            OnJumpAction();

        // 좌우
        _leftPressed = Input.GetKey(KeyCode.LeftArrow);
        _rightPressed = Input.GetKey(KeyCode.RightArrow);
        if(CanControl())
            _hPoint = (_leftPressed == true ? -1 : 0) + (_rightPressed == true ? 1 : 0);

        _hPoint = _hPoint / (_xAxisDrag + 1.0f);
    }
}
