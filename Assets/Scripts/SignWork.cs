using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignWork : MonoBehaviour
{
    public int posX;
    public int posY;
    public GameObject[] targets;
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < targets.Length; i++)
            targets[i].SetActive(false);
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.x > posX)
        {
            for (int i = 0; i < targets.Length; i++)
                targets[i].SetActive(true);
        }
    }
}
