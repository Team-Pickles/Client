using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnGround : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "floor":
            case "barricade":
            {
                transform.parent.GetComponent<EnemyAction>().OnGround = true;
                break;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "floor":
            case "barricade":
            {
                transform.parent.GetComponent<EnemyAction>().OnGround = true;
                break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "floor":
            case "barricade":
            {
                transform.parent.GetComponent<EnemyAction>().OnGround = false;
                break;
            }
        }
    }
}
