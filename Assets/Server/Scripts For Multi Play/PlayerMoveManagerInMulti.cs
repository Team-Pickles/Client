using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManagerInMulti : MonoBehaviour
{
    public Transform camTransform;
    public GameObject _player;
    public bool isPressed = false;
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("test");
        if (collision.gameObject.CompareTag("Item") && isPressed)
        {
            ItemManager im = collision.gameObject.GetComponent<ItemManager>();
            im.Collide();
            ClientSend.ItemCollide(im.id);
        }
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.RightArrow),
            Input.GetKey(KeyCode.LeftArrow),
            Input.GetKey(KeyCode.UpArrow),
            Input.GetKey(KeyCode.X),
        };
        ClientSend.PlayerMovement(_inputs);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ClientSend.PlayerThrowGrenade(_player.transform.right);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ClientSend.PlayerShoot(_player.transform.right);
        }

        if (Input.GetKeyDown(KeyCode.A)) 
        {
            if (isPressed == false)
            {
                ClientSend.PlayerStartVaccume(_player.transform.right);
                isPressed = true;
            }
                
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            if (isPressed == true)
            {
                ClientSend.PlayerEndVaccume();
                isPressed = false;
            }
            
        }
    }
}