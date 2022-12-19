using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManagerInMulti : MonoBehaviour
{
    public Transform camTransform;
    public GameObject _player;
    public PlayerManager _playerManager;
    public bool isPressed = false;

    private void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.CompareTag("trash") && isPressed)
        {
            ItemManager im = collision.gameObject.GetComponent<ItemManager>();
            im.Collide();
            ClientSend.ItemCollide(im.id);
        }

    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("trash") && isPressed)
        {
            ItemManager im = other.gameObject.GetComponent<ItemManager>();
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
            Input.GetKey(KeyCode.DownArrow),
        };
        ClientSend.PlayerMovement(_inputs, GetComponent<SpriteRenderer>().flipX);
    }

    public void Update()
    {
        SendInputToServer();

        if (Input.GetKeyDown(KeyCode.S))
        {
            ClientSend.PlayerShootBullet(_player.transform.right);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ClientSend.PlayerShootGrenade(_player.transform.right);
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

        if (Input.GetKeyDown(KeyCode.D))
        {
            ClientSend.PlayerJump();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && ! _playerManager.onRope)
        {
            ClientSend.PlayerRopeMove();
        }


    }
}
