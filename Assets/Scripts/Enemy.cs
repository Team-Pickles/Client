using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
        if (state == EnemyState.Captive && collision.transform.name == "Player" && _isDead == false)
        {
            _isDead = true;
            Debug.Log(collision.transform.name + "¿¡°Ô ¸ÔÇûÀ½");

            collision.transform.GetComponent<PlayerMoveManager>().IncreaseBullet();
            Destroy(gameObject);
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
