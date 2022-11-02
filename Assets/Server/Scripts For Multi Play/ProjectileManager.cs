using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public int id;
    public MeshRenderer projectileModel;
    public GameObject explosionPrefab;


    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Explode(Vector3 _position, List<Vector3Int> _explodedPositions)
    {
        transform.position = _position;
        foreach(Vector3Int _explodedPosition in _explodedPositions) {
            GameManagerInServer.instance.Tilemap.SetTile(_explodedPosition, null);
        }
        //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameManagerInServer.projectiles.Remove(id);
        Destroy(gameObject);
    }
}

