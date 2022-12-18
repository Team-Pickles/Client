using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    public AudioClip SpringSound, PlayerJumpSound, PlayerShootSound, PlayerDamagedSound, PlayerPickupSound, EnemyDamagedSound, GameOverSound, StageClearSound;

    public void PlaySound(string action)
    {
        switch(action)
        {
            case "EnemyDamaged":
                _audioSource.clip = EnemyDamagedSound;
                break;
            case "PlayerDamaged":
                _audioSource.clip = PlayerDamagedSound;
                break;
            case "PlayerJump":
                _audioSource.clip = PlayerJumpSound;
                break;
            case "PlayerPickup":
                _audioSource.clip = PlayerPickupSound; // 소리가... 다른 소리 없나...
                break;
            case "PlayerShoot":
                _audioSource.clip = PlayerShootSound;
                break;
            case "PlayerSpring":
                _audioSource.clip = SpringSound;
                break;
            default:
                _audioSource.clip = null;
                break;
        }
        _audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
