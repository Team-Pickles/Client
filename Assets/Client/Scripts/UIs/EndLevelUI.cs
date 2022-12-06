using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelUI : MonoBehaviour
{
    public GameObject text;
    private GameObject _fadeUI;
    public void EndLevel()
    {
        StartCoroutine(ImageShow());
        _fadeUI.GetComponent<FadeUI>().FadeIn(1.0f);
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
        _fadeUI = GameObject.Find("FadeUI");
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
