using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEnemy : MonoBehaviour
{
    public int id;
    private bool _isDead = false;
    private int _hitPoint = 1;
    enum EnemyState
    {
        Normal, Captive
    }
    private EnemyState state = EnemyState.Normal;

    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Collide()
    {
        GameManagerInServer.enemies.Remove(id);
        Destroy(gameObject);
    }
}