using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    public GameObject obj;
    public float spawnTime;
    bool IsAlive()
    {
        return true;
    }
    IEnumerator Spawn()
    {
        while(IsAlive())
        {
            Instantiate(obj, transform.position, new Quaternion());
            yield return new WaitForSeconds(spawnTime);
        }
        yield return 0;
    }
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
