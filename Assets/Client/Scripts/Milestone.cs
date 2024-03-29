using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milestone : MonoBehaviour
{
    public Transform toward;
    public bool isDestroy;
    public float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Spring(Clone)")
        {
            GameObject activedObj = collision.gameObject;
            if (isDestroy)
            {
                Destroy(activedObj);
            }
            activedObj.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            Vector2 direction = (toward.position - transform.position).normalized;
            activedObj.GetComponent<Rigidbody2D>().AddForce(direction * speed);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
