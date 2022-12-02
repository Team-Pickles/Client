using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Animator _animator;
    private ParticleSystem _ps;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _animator.SetBool("isCollide", true);
    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        _ps = GetComponent<ParticleSystem>();
    }
    public void AnimationEnd()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        GetComponent<CircleCollider2D>().enabled = false;
        _ps.Play();
    }
    public void AnimationEndDestroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    void Update()
    {
        
    }
}
