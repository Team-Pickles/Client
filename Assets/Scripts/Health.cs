using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int _health = 3;
    private int _numOfHearts = 3;
    private GameObject _player;

    //private�� ������ ������ ������δ� �� �ǳ�...
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    void Update()
    {
        _player = GameObject.Find("Player");
        PlayerMoveManager _ppm = _player.GetComponent<PlayerMoveManager>();
        _health = _ppm.Hp;
        if (_health > _numOfHearts)
            _health = _numOfHearts;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
            if (i < _numOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }
}
