using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public int id;
    public MeshRenderer bulletModel;
    public GameObject explosionPrefab;


    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Collide()
    {
        GameManagerInServer.projectiles.Remove(id);
        Destroy(gameObject);
    }


}
