using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public int health;
    public int maxHealth = 3;
    public SpriteRenderer model;
    public int BulletCount = 0;
    public int GrenadeCount = 0;
    public bool onRope = false;

    public void Initialize(int _id, string _username)
    {
        maxHealth = 3;
        id = _id;
        username = _username;
        health = maxHealth;

    }

    public void setHealth(int _health)
    {
        health = _health;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        setHealth(maxHealth);
    }
}
