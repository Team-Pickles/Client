using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashRed : MonoBehaviour
{
    enum TrashState
    {
        Normal, Captive
    }
    private TrashState state = TrashState.Normal;
    private Vector3 _position;
    public GameObject TrashEnemyPrefab;
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
        if(state == TrashState.Normal && collision.transform.tag == "floor")
        {
            _position = new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2.0f) * 3.0f, 0);
            Destroy(gameObject);
            GameObject TrashEnemy = Object.Instantiate(TrashEnemyPrefab, _position, new Quaternion());
        }
        if (state == TrashState.Captive && collision.transform.name == "Player")
        {
            Debug.Log(collision.transform.name + "¿¡°Ô ¸ÔÇûÀ½");

            collision.transform.GetComponent<PlayerMoveManager>().IncreaseBullet();
            Destroy(gameObject);
        }
    }
    void Start()
    {
        TrashEnemyPrefab = (GameObject)Resources.Load("Prefabs/Enemies/TrashEnemy", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
