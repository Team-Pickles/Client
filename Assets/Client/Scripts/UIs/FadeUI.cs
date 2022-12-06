using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
	private Image _image;
	public void FadeIn(float fadeTime)
    {
        StartCoroutine(CoFadeIn(fadeTime));
    }
    public void FadeOut(float fadeTime)
    {
		StartCoroutine(CoFadeOut(fadeTime));
    }
	private IEnumerator CoFadeIn(float fadeTime)
	{
		Color tempColor = _image.color;
		while (tempColor.a < 1f)
		{
			tempColor.a += Time.deltaTime / fadeTime;
			_image.color = tempColor;

			if (tempColor.a >= 1f) tempColor.a = 1f;

			yield return null;
		}

		_image.color = tempColor;
	}
	private IEnumerator CoFadeOut(float fadeTime)
	{
		Color tempColor = _image.color;
		while (tempColor.a > 0f)
		{
			tempColor.a -= Time.deltaTime / fadeTime;
			_image.color = tempColor;

			if (tempColor.a <= 0f) tempColor.a = 0f;

			yield return null;
		}
		_image.color = tempColor;
	}
	// Start is called before the first frame update
	void Start()
    {
		_image = GetComponentInChildren<Image>();
	}

    // Update is called once per frame
    void Update()
    {

    }
}