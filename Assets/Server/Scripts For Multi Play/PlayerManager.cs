using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth;
    public SpriteRenderer model;
    public float itemCount = 0;
    public bool onRope = false;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

    }

    public void setHealth(float _health)
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
