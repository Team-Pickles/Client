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

    public void OnDamaged()
    {
        StartCoroutine(Damaged());
    }

    private IEnumerator Damaged()
    {
        for (int i = 0; i < 6; i++)
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

        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.7f;
            GetComponent<SpriteRenderer>().color = color;

            yield return new WaitForSeconds(0.7f);

            color.a = 1.0f;
            GetComponent<SpriteRenderer>().color = color;
        }
        yield break;
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
