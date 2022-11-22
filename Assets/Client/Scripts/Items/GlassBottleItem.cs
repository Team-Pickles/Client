using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBottleItem : MonoBehaviour
{
    private GameObject _player;
    private bool touchItem = false;
    SpellManager sm;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            touchItem = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            touchItem = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        sm = _player.GetComponent<SpellManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchItem)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {

                sm.ChangeSkill(new GlassBottle());
                Destroy(gameObject);
            }
        }
    }
}
