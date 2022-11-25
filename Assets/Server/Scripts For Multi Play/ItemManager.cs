using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int id;


    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Collide()
    {
        GameManagerInServer.items.Remove(id);
        Destroy(gameObject);
    }



}
