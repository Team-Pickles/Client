using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
	private Color _tempColor;
	private Image _image;
	public void FadeIn(float fadeTime)
    {
        StartCoroutine(CoFadeIn(fadeTime));
		StopCoroutine(CoFadeIn(fadeTime));
    }
	public void FadeOut(float fadeTime)
	{
		StartCoroutine(CoFadeOut(fadeTime));
		StopCoroutine(CoFadeOut(fadeTime));
	}
	public IEnumerator CoFadeIn(float fadeTime)
	{
		_image = GetComponentInChildren<Image>();
		_tempColor = _image.color;
		_tempColor.a = 0f;
		while (_tempColor.a < 1f)
		{
			_tempColor.a += Time.deltaTime / fadeTime;
			_image.color = _tempColor;

			if (_tempColor.a >= 1f) _tempColor.a = 1f;

			yield return null;
		}
		_image.color = _tempColor;
	}
	public IEnumerator CoFadeOut(float fadeTime)
	{
		_image = GetComponentInChildren<Image>();
		_tempColor = _image.color;
		_tempColor.a = 1f;
		while (_tempColor.a > 0f)
		{
			_tempColor.a -= Time.deltaTime / fadeTime;
			_image.color = _tempColor;

			if (_tempColor.a <= 0f) _tempColor.a = 0f;

			yield return null;
		}
		_image.color = _tempColor;
	}
	// Start is called before the first frame update
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {

    }
}