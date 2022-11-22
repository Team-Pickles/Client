using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBottleItem : MonoBehaviour
{
    public GameObject player;
    private GameObject _player;
    private PlayerMoveManager _pmm;
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
        _pmm = _player.GetComponent<PlayerMoveManager>();
        sm = player.GetComponent<SpellManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchItem)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {

                sm.ChangeSkill(new GlassBottle());
                _pmm.IncreaseGlassBottle(3);
                Destroy(gameObject);
            }
        }
    }
}
