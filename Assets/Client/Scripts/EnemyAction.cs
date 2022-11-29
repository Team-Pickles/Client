using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    [SerializeField] private bool isMove;
    private GameObject _player;
    private bool _onGround = false;
    private Color _tintColor = new Color(1.0f, 0.2f, 0.2f, 1.0f);
    private ParticleSystem _ps;
    private Animator _animator;
    public bool OnGround
    {
        get { return _onGround; }
        set { _onGround = value; }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "bullet":
            {
                StartCoroutine(HitAction());
                break;
            }
        }
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
    private bool DetectPlayer()
    {
        Vector2 diff = _player.transform.position - transform.position;
        float distance = diff.magnitude;
        return distance <= 8.0f;
    }
    private void MoveEnemy()
    {
        if (_onGround)
        {
            if (DetectPlayer())
            {
                _animator.SetBool("isMoving", true);

                if (_player.transform.position.x < transform.position.x) // left
                {
                    Vector2 scale = transform.localScale;
                    scale.x = scale.x > 0.0f ? scale.x : -scale.x;
                    transform.localScale = scale;

                    GetComponent<Rigidbody2D>().velocity = new Vector2(-6.0f, 4.8f);
                }
                else // right
                {
                    Vector2 scale = transform.localScale;
                    scale.x = scale.x > 0.0f ? -scale.x : scale.x;
                    transform.localScale = scale;

                    GetComponent<Rigidbody2D>().velocity = new Vector2(6.0f, 4.8f);
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
        _player = GameObject.Find("Player");
        _ps = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isMove)
            MoveEnemy();
    }
}
