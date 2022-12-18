using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringAction : MonoBehaviour
{
    public AudioClip sound;
    private Color _tintColor = new Color(1.0f, 0.5f, 0.5f, 1.0f);
    private ParticleSystem _ps;
    private AudioSource _audio;
    private void PlaySound()
    {
        _audio.clip = sound;
        _audio.Play();
    }
    private IEnumerator ChangeColor()
    {
        float time = 0.0f;
        while (time <= 0.5f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(_tintColor, Color.white, time*2.0f);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "player":
            {
                PlayerMoveManager pmm;
                // player check, not collider.
                if (collision.TryGetComponent<PlayerMoveManager>(out pmm))
                {
                    pmm.OnJumpSpringAction();
                    PlaySound();
                    //_ps.Play();
                    StartCoroutine(ChangeColor());
                }
                break;
            }
            case "barricade":
            {
                Vector2 velocity = collision.transform.GetComponent<Rigidbody2D>().velocity;
                collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, 10.0f);
                break;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "barricade":
            {
                Vector2 velocity = collision.transform.GetComponent<Rigidbody2D>().velocity;
                collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, 10.0f);
                break;
            }
        }
    }
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
