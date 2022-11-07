using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSpring : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.name)
        {
            case "Player":
            {
                collision.GetComponent<PlayerMoveManager>().OnJumpSpringAction();
                break;
            }
        }
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
