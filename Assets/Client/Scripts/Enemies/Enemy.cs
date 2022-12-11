using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool _isDead = false;
    private int _hitPoint = 1;
    enum EnemyState
    {
        Normal, Captive
    }
    private EnemyState state = EnemyState.Normal;
    public void OnCaptive()
    {
        Debug.Log("Enemy::Captive");
        state = EnemyState.Captive;
    }
    public void OnReleased()
    {
        Debug.Log("Enemy::Released");
        state = EnemyState.Normal;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Bullet(Clone)" && _isDead == false)
        {
            Destroy(collision.transform.gameObject);
            _hitPoint -= 1;
            if (_hitPoint <= 0)
            {
                _isDead = true;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Bullet(Clone)" && _isDead == false)
        {
            Destroy(collision.transform.gameObject);
            _hitPoint -= 1;
            if (_hitPoint <= 0)
            {
                _isDead = true;
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (state == EnemyState.Normal && collision.transform.name == "Player" && _isDead == false)
        {
            collision.transform.GetComponent<PlayerMoveManager>().OnDamagedAction();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (state == EnemyState.Normal && collision.transform.name == "Player" && _isDead == false)
        {
            collision.transform.GetComponent<PlayerMoveManager>().OnDamagedAction();
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
