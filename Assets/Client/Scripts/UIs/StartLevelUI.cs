using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelUI : MonoBehaviour
{
    private GameObject _fadeUI;
    public GameObject text;
    // Start is called before the first frame update
    public IEnumerator ImageShow()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        text.SetActive(false);
        yield return null;
    }
    void Start()
    {
        _fadeUI = GameObject.Find("FadeUI");
        _fadeUI.GetComponent<FadeUI>().FadeOut(1.0f);
        StartCoroutine(ImageShow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
