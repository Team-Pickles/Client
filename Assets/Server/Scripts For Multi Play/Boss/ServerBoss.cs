using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerBoss : MonoBehaviour
{
    public int id;
    private bool _isDead = false;
    private int _hitPoint = 1;
    private Color _tintColor = new Color(1.0f, 0.2f, 0.2f, 1.0f);

    private void Start()
    {

    }

    public void Initialize(int _id)
    {
        id = _id;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void BossHit()
    {
        StartCoroutine(HitAction());
    }

    IEnumerator HitAction()
    {
        for (int i = 0; i < 6; i++)
        {
            Color color = transform.GetChild(1).GetComponent<SpriteRenderer>().color;
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
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.1f);
        }

        {
            Color color = transform.GetChild(1).GetComponent<SpriteRenderer>().color;
            color.a = 0.7f;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;

            yield return new WaitForSeconds(0.7f);

            color.a = 1.0f;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
        }

        yield break;
    }
}