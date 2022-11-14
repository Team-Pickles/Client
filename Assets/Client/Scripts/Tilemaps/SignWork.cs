using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignWork : MonoBehaviour
{
    private bool _notSeen = true, _isOpen = false;
    public GameObject panel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player" && _notSeen && !_isOpen)
        {
            _notSeen = false;
            _isOpen = true;
            panel.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name == "Player" && Input.GetKey(KeyCode.W) && !_isOpen)
        {
            _isOpen = true;
            panel.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && _isOpen)
        {
            _isOpen = false;
            panel.SetActive(false);
        }
    }
}
