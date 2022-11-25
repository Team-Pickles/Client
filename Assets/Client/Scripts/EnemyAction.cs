using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    private Color _tintColor = new Color(1.0f, 0.2f, 0.2f, 1.0f);
    private ParticleSystem _ps;

    IEnumerator HitAction()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        float time = 0.0f;
        while (time <= 0.5f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, _tintColor, time*2.0f);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GetComponent<SpriteRenderer>().enabled = false;
        _ps.Play();
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "bullet":
            {
                StartCoroutine(HitAction());
                break;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
            {
                collision.transform.GetComponent<PlayerMoveManager>().OnDamagedAction();
                break;
            }
            case "bullet":
            {
                StartCoroutine(HitAction());
                break;
            }
        }
    }
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
