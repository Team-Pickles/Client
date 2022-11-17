using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignWork : MonoBehaviour
{
    private bool _isOpen = false;
    public GameObject panel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player" && !_isOpen)
        {
            _isOpen = true;
            panel.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "Player" && _isOpen)
        {
            _isOpen = false;
            panel.SetActive(false);
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

    }
}
