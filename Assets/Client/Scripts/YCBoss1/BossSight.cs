using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossSight : MonoBehaviour
{
    private BossAlertUI _bossAlertUI;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.name == "Player")
        {
            transform.parent.GetComponent<BossMoveManager>().PlayerInSight = true;
            Destroy(gameObject);
            _bossAlertUI.BossAlert();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _bossAlertUI = GameObject.Find("BossAlertUI").GetComponent<BossAlertUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
