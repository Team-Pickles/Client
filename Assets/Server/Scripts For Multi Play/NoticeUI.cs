using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour
{
    [Header("SubNotice")]
    public GameObject box;
    public Text text;
    public Animator ani;

    private WaitForSeconds Delay1 = new WaitForSeconds(2.0f);
    private WaitForSeconds Delay2 = new WaitForSeconds(0.3f);
    
    void Start()
    {
        box.SetActive(false);
    }

    public void AlertBox(string _msg)
    {
        text.text = _msg;
        box.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        box.SetActive(true);
        ani.SetBool("isShown", true);
        yield return Delay1;
        ani.SetBool("isShown", false);
        yield return Delay1;
        box.SetActive(false);
    }
}
