using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashAction : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "player")
        {
            for (int i = 0; i < 100; i++)
            {
                collision.transform.GetComponent<PlayerMoveManager>().IncreaseBullet(1);
            }
            Destroy(gameObject);

            }
        }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
