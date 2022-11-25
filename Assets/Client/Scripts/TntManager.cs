using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TntManager : MonoBehaviour
{
    private GameObject _player;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
            {
                Vector2 value = (_player.transform.position - transform.position).normalized * 4.0f;
                _player.GetComponent<PlayerMoveManager>().OnExplosionAction(value.x, value.y);
                //Destroy(this.gameObject);
                break;
            }
        }
    }
    void Start()
    {
        _player = GameObject.Find("Player");

    }
}
