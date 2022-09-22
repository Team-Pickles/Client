using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    enum EnemyState
    {
        Normal, Captive
    }
    private EnemyState state = EnemyState.Normal;
    public void OnCaptive()
    {
        state = EnemyState.Captive;
    }
    public void OnReleased()
    {
        state = EnemyState.Normal;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == EnemyState.Captive)// && collision.transform.CompareTag("player"))
        {
            Debug.Log("123");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
