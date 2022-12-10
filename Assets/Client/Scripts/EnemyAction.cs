using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    [SerializeField] private bool isMove;
    private GameObject _player;
    private float _xSpeed = 5.0f;
    private bool _onGround = false;
    private bool _detectPlayer = false;
    private Color _tintColor = new Color(1.0f, 0.2f, 0.2f, 1.0f);
    private ParticleSystem _ps;
    private Animator _animator;
    public bool OnGround
    {
        get { return _onGround; }
        set { _onGround = value; }
    }
    public bool DetectPlayer
    {
        get { return _detectPlayer; }
        set { _detectPlayer = value; }
    }
    public GameObject DetectedPlayer
    {
        get { return _player; }
        set { _player = value; }
    }

    IEnumerator HitAction()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        float time = 0.0f;
        while (time <= 0.5f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, _tintColor, time*2.0f);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GetComponent<SpriteRenderer>().enabled = false;
        _ps.Play();
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
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
                StartCoroutine(HitAction());
                break;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
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
                StartCoroutine(HitAction());
                break;
            }
        }
    }
    private void MoveEnemy()
    {
        if (_onGround)
        {
            if (_detectPlayer && _player != null)
            {
                _animator.SetBool("isMoving", true);

                if (_player.transform.position.x < transform.position.x) // left
                {
                    Vector2 scale = transform.localScale;
                    scale.x = scale.x > 0.0f ? scale.x : -scale.x;
                    transform.localScale = scale;

                    GetComponent<Rigidbody2D>().velocity = new Vector2(-_xSpeed, 4.8f);
                }
                else // right
                {
                    Vector2 scale = transform.localScale;
                    scale.x = scale.x > 0.0f ? -scale.x : scale.x;
                    transform.localScale = scale;

                    GetComponent<Rigidbody2D>().velocity = new Vector2(_xSpeed, 4.8f);
                }
            }
            else
            {
                _animator.SetBool("isMoving", false);
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            }
        }
    }

    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isMove)
            MoveEnemy();
    }
}
