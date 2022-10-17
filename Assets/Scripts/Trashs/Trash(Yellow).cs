using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashYellow : MonoBehaviour
{
    enum TrashState
    {
        Normal, Captive
    }
    private TrashState state = TrashState.Normal;
    public void OnCaptive()
    {
        Debug.Log("Enemy::Captive");
        state = TrashState.Captive;
    }
    public void OnReleased()
    {
        Debug.Log("Enemy::Released");
        state = TrashState.Normal;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (state == TrashState.Captive && collision.transform.name == "Player")
        {
            Debug.Log(collision.transform.name + "¿¡°Ô ¸ÔÇûÀ½");

            collision.transform.GetComponent<PlayerMoveManager>().IncreaseBullet();
            Destroy(gameObject);
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
