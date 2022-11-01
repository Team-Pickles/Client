using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int id;
    public MeshRenderer projectileModel;

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
