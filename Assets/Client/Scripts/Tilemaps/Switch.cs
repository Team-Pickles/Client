using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private bool _state = false;
    public GameObject onBlock, offBlock;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Bullet(Clone)")
        {
            _state = !_state;
            Destroy(collision.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            _state = true;
        if (Input.GetKeyDown(KeyCode.P))
            _state = false;
        onBlock.SetActive(_state);
        offBlock.SetActive(!_state);
    }
}
