using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossInfoUI : MonoBehaviour
{
    public GameObject HpBar;
    private int _bossFullHp, _bossCurrentHp;
    private Slider _slider;
    private GameObject _boss;
    private BossMoveManager _bmm;

    public void BossIncoming()
    {
        StartCoroutine(Animation());
    }
    public IEnumerator Animation()
    {
        HpBar.SetActive(true);
        double value = 0;
        _slider.value = 0;
        while(value < 1f)
        {
            value += Time.deltaTime / 1.0;
            _slider.value = (float)value;
            if (value >= 1f) value = 1f;
            yield return null;
        }
        _slider.value = (float)value;
        StartCoroutine(ShowHp());
    }
    public IEnumerator ShowHp()
    {
        _bossFullHp = _bmm.Hp;
        _bossCurrentHp = _bmm.Hp;
        double value = _bossCurrentHp / (double)_bossFullHp;
        while (_bossCurrentHp > 0)
        {
            _bossCurrentHp = _bmm.Hp;
            value = _bossCurrentHp / (double)_bossFullHp;
            _slider.value = (float)value;
            yield return null;
        }
        _slider.value = (float)value;
        HpBar.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        HpBar.SetActive(false);
        _slider = HpBar.GetComponentInChildren<Slider>();
        _boss = GameObject.Find("Boss");
        _bmm = _boss.GetComponent<BossMoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            BossIncoming();
    }
}
