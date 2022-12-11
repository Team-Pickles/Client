using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAlertUI : MonoBehaviour
{
    public GameObject text;
    private BossInfoUI _bossInfoUI;
    public void BossAlert()
    {
        StartCoroutine(ImageShow());
        _bossInfoUI.BossIncoming();
    }
    public IEnumerator ImageShow()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        text.SetActive(false);
       
        
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
        _bossInfoUI = GameObject.Find("BossInfoUI").GetComponent<BossInfoUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
