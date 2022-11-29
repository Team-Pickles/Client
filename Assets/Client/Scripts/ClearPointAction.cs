using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPointAction : MonoBehaviour
{
    public Transform nextLevel;
    public GameManager gm;
    private GameObject _player;
    private bool _canClear = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            _canClear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            _canClear = false;
        }
    }
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_canClear && Input.GetKeyDown(KeyCode.UpArrow))
        {
            _player.transform.position = nextLevel.position;
            gm.SetResurrectionPoint(nextLevel.position);
        }
    }
}
