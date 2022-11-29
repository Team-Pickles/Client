using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrashYellow : MonoBehaviour
{
    private GameObject _tilemap;
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
        if (state == TrashState.Normal && collision.transform.tag == "floor")
        {
            _tilemap = GameObject.Find("Tilemap_ground");
            for (int i = -3; i <= 3; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    Vector3Int position = new Vector3Int((int)transform.position.x + i, (int)transform.position.y + j, 0);
                    _tilemap.GetComponent<Tilemap>().SetTile(position, null);
                }
            }
            Destroy(gameObject);
        }
        if (state == TrashState.Captive && collision.transform.name == "Player")
        {
            Debug.Log(collision.transform.name + "¿¡°Ô ¸ÔÇûÀ½");

            collision.transform.GetComponent<PlayerMoveManager>().IncreaseBullet(1);
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
