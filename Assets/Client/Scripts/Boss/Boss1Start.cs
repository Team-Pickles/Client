using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Start : MonoBehaviour
{
    public Boss1 boss;
    private BossAlertUI _bossAlertUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            boss.SetState(new Boss1Idle(boss));
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
