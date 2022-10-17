using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingEnemy : MonoBehaviour
{
    private bool _ready = true;
    private GameObject _player;
    public GameObject canPrefab;

    public IEnumerator OnAttack()
    {
        GameObject can = Object.Instantiate(canPrefab, new Vector3(transform.position.x + transform.localScale.x / 2.0f * 0.8f, transform.position.y - transform.localScale.x / 2.0f * 0.8f, 0), new Quaternion());
        can.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(250.0f, 0.0f));
        yield return new WaitForSeconds(3.0f);
        Object.Destroy(can);
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
        canPrefab = (GameObject)Resources.Load("Prefabs/EnemyItems/Can", typeof(GameObject));
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
