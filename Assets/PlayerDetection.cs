using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
            {
                transform.parent.GetComponent<EnemyAction>().DetectPlayer = true;
                transform.parent.GetComponent<EnemyAction>().DetectedPlayer = collision.gameObject;
                break;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
            {
                transform.parent.GetComponent<EnemyAction>().DetectPlayer = true;
                transform.parent.GetComponent<EnemyAction>().DetectedPlayer = collision.gameObject;
                break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
            {
                transform.parent.GetComponent<EnemyAction>().DetectPlayer = false;
                transform.parent.GetComponent<EnemyAction>().DetectedPlayer = null;
                break;
            }
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
