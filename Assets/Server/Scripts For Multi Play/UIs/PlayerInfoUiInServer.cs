using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUiInServer : MonoBehaviour
{
    private int _healthCount, _bulletCount, _grenadeCount, _tens, _ones;
    private PlayerManager _pmm;
    public Sprite[] _numberDigit;
    public Image heartTenDigit, heartOneDigit, bulletTenDigit, bulletOneDigit, grenadeTenDigit, grenadeOneDigit;
    // Start is called before the first frame update
    public void Initialize(PlayerManager _player)
    {
        _pmm = _player;
    }

    // Update is called once per frame
    void Update()
    {
        _healthCount = _pmm.health;
        _tens = _healthCount / 10;
        _ones = _healthCount % 10;
        heartTenDigit.sprite = _numberDigit[_tens];
        heartOneDigit.sprite = _numberDigit[_ones];
        if (_tens == 0)
            heartTenDigit.enabled = false;
        else
            heartTenDigit.enabled = true;
        heartOneDigit.enabled = true;

        _bulletCount = _pmm.BulletCount;
        _tens = _bulletCount / 10;
        _ones = _bulletCount % 10;
        bulletTenDigit.sprite = _numberDigit[_tens];
        bulletOneDigit.sprite = _numberDigit[_ones];
        if (_tens == 0)
            bulletTenDigit.enabled = false;
        else
            bulletTenDigit.enabled = true;
        bulletOneDigit.enabled = true;

        _grenadeCount = _pmm.GrenadeCount;
        _tens = _grenadeCount / 10;
        _ones = _grenadeCount % 10;
        grenadeTenDigit.sprite = _numberDigit[_tens];
        grenadeOneDigit.sprite = _numberDigit[_ones];
        if (_tens == 0)
            grenadeTenDigit.enabled = false;
        else
            grenadeTenDigit.enabled = true;
        grenadeOneDigit.enabled = true;
    }
}
