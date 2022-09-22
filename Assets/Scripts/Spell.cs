using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        switch (collision.tag)
        {
            case "enemy":
            {
                collision.attachedRigidbody.velocity = new Vector2(-1.0f, 0.0f);
                break;
            }
        }
    }
    void Start()
    {
        
    }

    GameObject enemy;
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Vector2 ro = new Vector2(transform.parent.position.x + transform.parent.localScale.x / 2.0f * 1.1f, transform.parent.position.y);
            float length = 8.0f;

            for (int i = -5; i <= 5; i++)
            {
                Vector2 rd = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad));
                RaycastHit2D hit = Physics2D.Raycast(ro, rd, length);

                if (hit.collider != null)
                {
                    Debug.DrawLine(ro, hit.point, Color.green);
                    if (hit.transform.CompareTag("enemy"))
                    {
                        enemy = hit.transform.gameObject;
                        enemy.GetComponent<Enemy>().OnCaptive();
                    }
                    if (hit.rigidbody != null)
                        hit.rigidbody.AddForce((hit.point - ro).normalized * -0.5f);
                }
            }
        }
        else
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().OnReleased();
                enemy = null;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
