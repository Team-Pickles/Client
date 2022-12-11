using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject text, gameOverText;
    private GameObject _player;
    private PlayerMoveManager _pmm;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _pmm = _player.GetComponent<PlayerMoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_pmm.Hp == 0)
        {
            text.SetActive(true);
            gameOverText.SetActive(true);
        }
        else
        {
            text.SetActive(false);
            gameOverText.SetActive(false);
        }
    }
}
