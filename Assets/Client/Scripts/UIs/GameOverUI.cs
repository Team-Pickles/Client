using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject text, gameOverText;
    public AudioClip sound;
    private GameObject _player;
    private PlayerMoveManager _pmm;
    private AudioSource _audio;
    private bool _hidden = true;
    private void PlaySound()
    {
        _audio.clip = sound;
        _audio.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _pmm = _player.GetComponent<PlayerMoveManager>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_pmm.Hp == 0)
        {
            if(_hidden)
                PlaySound();
            _hidden = false;
            text.SetActive(true);
            gameOverText.SetActive(true);
        }
        else
        {
            text.SetActive(false);
            gameOverText.SetActive(false);
            _hidden = true;
        }
    }
}
