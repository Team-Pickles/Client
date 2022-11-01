using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public GameObject rainStart;
    public GameObject trash;
    public GameObject enemy;
    public GameObject barrior;

    public GameObject player;
    public GameObject clearPosition;

    private Boss1State _state;
    private bool _immortal = false;
    public int hp = 5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_immortal && collision.name == "Bullet(Clone)")
        {
            Destroy(collision.gameObject);
            StartCoroutine(Damaged());
        }
    }
    private IEnumerator Damaged()
    {
        _immortal = true;
        hp--;
        Debug.Log(hp + " 남았습니다");
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
        // immortal state
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.7f;
            GetComponent<SpriteRenderer>().color = color;

            yield return new WaitForSeconds(0.7f);

            color.a = 1.0f;
            GetComponent<SpriteRenderer>().color = color;
        }
        _immortal = false;
        yield break;
    }
    public void OnEnd()
    {
        player.GetComponent<Rigidbody2D>().position = (clearPosition.transform.position);
        Destroy(gameObject);
    }
    public void SetState(Boss1State state)
    {
        _state = state;
        StartCoroutine(_state.Start());
    }
    private void Start()
    {
        SetState(new Boss1Idle(this));
    }
}
