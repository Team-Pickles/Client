using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss1 : MonoBehaviour
{
    public GameObject attackRange;
    public string nextLevel;
    [HideInInspector] public GameObject trash;
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public GameObject barrior;

    private Boss1State _state;
    private bool _immortal = false;
    [HideInInspector] public int hp = 10;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_immortal && collision.transform.name == "Bullet(Clone)")
        {
            StartCoroutine(Damaged());
            if (hp == 0)
            {
                StartCoroutine(Clear());
            }
        }
    }
    private IEnumerator Clear()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(nextLevel);
        GameObject.Find("Player").transform.position = new Vector3(-10.0f, 2.0f, 0.0f);
    }
    private IEnumerator Damaged()
    {
        _immortal = true;
        hp--;
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
    public void SetState(Boss1State state)
    {
        _state = state;
        StartCoroutine(_state.Start());
    }
    private void Start()
    {
        trash = Resources.Load("Prefabs/Trash") as GameObject;
        enemy = Resources.Load("Prefabs/Boss1/Boss1Enemy") as GameObject;
        barrior = Resources.Load("Prefabs/Boss1/Boss1Barrior") as GameObject;
    }
}
