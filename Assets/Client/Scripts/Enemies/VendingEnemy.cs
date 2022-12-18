using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingEnemy : MonoBehaviour
{
    private int _hp = 3;
    private bool _ready = true;
    private GameObject _canPrefab;
    private GameObject _firePoint;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
                {
                    collision.transform.GetComponent<PlayerMoveManager>().OnDamagedAction();
                    break;
                }
            case "bullet":
                {
                    _hp--;
                    if (_hp == 0)
                        Destroy(gameObject);
                    break;
                }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
                {
                    collision.transform.GetComponent<PlayerMoveManager>().OnDamagedAction();
                    break;
                }
        }
    }
    public IEnumerator OnAttack()
    {
        GameObject can = Object.Instantiate(_canPrefab, _firePoint.transform.position, new Quaternion());
        can.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-300.0f, 0.0f));
        yield return new WaitForSeconds(10.0f);
        Object.Destroy(can);
        yield return 0;
    }
    public IEnumerator Waiting(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _ready = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        _canPrefab = (GameObject)Resources.Load("Prefabs/EnemyItems/Can", typeof(GameObject));
        _firePoint = GameObject.Find("VendingEnemyFirePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if(_ready)
        {
            _ready = false;
            StartCoroutine(OnAttack());
            StartCoroutine(Waiting((float)(_hp)));
        }
    }
}
