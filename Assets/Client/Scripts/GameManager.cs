using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject _player;
    public Vector3 resurrectionPoint;
    private PlayerMoveManager pmm;

    public void SetResurrectionPoint(Vector3 point)
    {
        resurrectionPoint = point;
    }
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        pmm = _player.GetComponent<PlayerMoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pmm.Hp <= 0)
        {
            _player.SetActive(false);
        }
        if (_player.activeSelf == false) // Á×Àº »óÅÂ
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _player.transform.position = resurrectionPoint;
                pmm.ResetVariable();
                _player.SetActive(true);
            }
        }
    }
}
