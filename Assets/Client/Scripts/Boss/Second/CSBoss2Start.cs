using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSBoss2Start : MonoBehaviour
{
    public CSBoss2 boss;
    private BossAlertUI _bossAlertUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            boss.SetState(new CSBoss2Move(boss));
            _bossAlertUI.BossAlert();
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _bossAlertUI = GameObject.Find("BossAlertUI").GetComponent<BossAlertUI>();
    }
    void Update()
    {

    }
}
