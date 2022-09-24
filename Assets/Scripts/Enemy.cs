using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _hitpoint = 1;
    private bool _isDead = false;
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (state == EnemyState.Normal && collision.transform.name == "Player" && _isDead == false)
        {
            collision.transform.GetComponent<PlayerMoveManager>().OnDamagedAction();
        }
        if (state == EnemyState.Captive && collision.transform.name == "Player" && _isDead == false)
        {
            _isDead = true;
            Debug.Log(collision.transform.name + "���� ������");

            collision.transform.GetComponent<PlayerMoveManager>().IncreaseBullet();
            Destroy(gameObject);
        }
        if (collision.transform.name == "Bullet(Clone)" && _isDead == false)
        {
            Debug.Log(collision.transform.name + "�� ����");
            Destroy(collision.transform.gameObject);
            _hitpoint -= 1;
            if(_hitpoint <= 0)
            {
                _isDead = true;
                Destroy(gameObject);
            } 
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
