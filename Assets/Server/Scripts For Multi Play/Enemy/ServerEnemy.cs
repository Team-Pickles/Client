using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEnemy : MonoBehaviour
{
    public int id;
    private bool _isDead = false;
    private int _hitPoint = 1;
    public Animator _animator;
    private ParticleSystem _ps;
    private Color _tintColor = new Color(1.0f, 0.2f, 0.2f, 1.0f);

    enum EnemyState
    {
        Normal, Captive
    }
    private EnemyState state = EnemyState.Normal;

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
    }

    public void Initialize(int _id)
    {
        id = _id;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void EnemyHit()
    {
        StartCoroutine(HitAction());
    }

    IEnumerator HitAction()
    {
        float time = 0.0f;
        while (time <= 0.5f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, _tintColor, time * 2.0f);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GetComponent<SpriteRenderer>().enabled = false;
        _ps.Play();
        yield return new WaitForSeconds(4.0f);
    }
}