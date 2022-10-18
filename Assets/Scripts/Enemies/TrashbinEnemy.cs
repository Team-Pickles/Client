using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashbinEnemy : MonoBehaviour
{
    int choice;
    private bool _ready = true;
    public GameObject trashPrefab;
    public GameObject trashPrefab1;
    public GameObject trashPrefab2;
    public GameObject trashPrefab3;

    public IEnumerator OnAttack()
    {
        GameObject trash;
        trashPrefab = trashPrefab1;
        choice = Random.Range(0, 3);
        switch(choice)
        {
            case 0:
                trashPrefab = trashPrefab1;
                break;
            case 1:
                trashPrefab = trashPrefab2;
                break;
            case 2:
                trashPrefab = trashPrefab3;
                break;
        }
        trash = Object.Instantiate(trashPrefab, new Vector3(transform.position.x + transform.localScale.x / 2.0f * 0.8f, transform.position.y + transform.localScale.y / 2.0f * 1.1f, 0), new Quaternion());
        trash.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(200.0f, 300.0f));
        yield return new WaitForSeconds(3.0f);
        Object.Destroy(trash);
        yield return 0;
    }
    public IEnumerator Waiting()
    {
        yield return new WaitForSeconds(3.0f);
        _ready = true;
    }
    // Start is called before the first frame update
    void Awake()
    {
        trashPrefab1 = (GameObject)Resources.Load("Prefabs/EnemyItems/TrashWhite", typeof(GameObject));
        trashPrefab2 = (GameObject)Resources.Load("Prefabs/EnemyItems/TrashRed", typeof(GameObject));
        trashPrefab3 = (GameObject)Resources.Load("Prefabs/EnemyItems/TrashYellow", typeof(GameObject));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_ready)
        {
            _ready = false;
            StartCoroutine(OnAttack());
            StartCoroutine(Waiting());
        }
    }
}
