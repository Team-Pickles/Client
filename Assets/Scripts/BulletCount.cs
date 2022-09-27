using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCount : MonoBehaviour
{
    private int _bullet = 0;
    private int _maxBullet = 8;
    private GameObject _player;

    //private로 돌리고 싶은데 생각대로는 안 되네...
    public Image[] bullets;
    public Sprite fullBullet;
    public Sprite emptyBullet;
    void Update()
    {
        _player = GameObject.Find("Player");
        PlayerMoveManager _ppm = _player.GetComponent<PlayerMoveManager>();
        _bullet = _ppm.BulletCount;
        if (_bullet > _maxBullet)
            _bullet = _maxBullet;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (i < _bullet)
                bullets[i].sprite = fullBullet;
            else
                bullets[i].sprite = emptyBullet;
            if (i < _maxBullet)
                bullets[i].enabled = true;
            else
                bullets[i].enabled = false;
        }
    }
}
