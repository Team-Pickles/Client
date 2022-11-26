using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState { FadeIn = 0, FadeOut, PortalFade}
public class FadeUI : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10.0f)]
    private float fadeTime;
    [SerializeField]
    private AnimationCurve fadeCurve;
    private Image image;
    private FadeState fadeState;

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
            currentTime = Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }
    }

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch(fadeState)
        {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.PortalFade:
                StartCoroutine(PortalFade());
                break;
        }
    }
    private IEnumerator PortalFade()
    {
        bool enter = true;
        while(enter)
        {
            yield return StartCoroutine(Fade(1, 0));
            yield return StartCoroutine(Fade(0, 1));
            enter = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInChildren<Image>();
        OnFade(FadeState.FadeOut);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
