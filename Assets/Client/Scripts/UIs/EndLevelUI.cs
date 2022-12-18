using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelUI : MonoBehaviour
{
    public GameObject text;
    public AudioClip sound;
    private FadeUI _fadeUI;
    private AudioSource _audio;
    public void EndLevel()
    {
        StartCoroutine(ImageShow());
    }
    private void PlaySound()
    {
        _audio.clip = sound;
        _audio.Play();
    }
    public IEnumerator ImageShow()
    {
        PlaySound();
        text.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _fadeUI.FadeIn(1.0f);
        text.SetActive(false);
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _fadeUI = GameObject.Find("FadeUI").GetComponent<FadeUI>();
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
