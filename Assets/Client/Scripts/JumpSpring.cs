using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSpring : MonoBehaviour
{
    public PlayerMoveManager pmm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.name)
        {
            case "Player":
            {
                pmm.OnJumpSpringAction();
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
