using System;
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
		StartCoroutine(CoFadeIn(fadeTime, false, null));
		StopCoroutine(CoFadeIn(fadeTime, false, null));
	}
	public void FadeOut(float fadeTime)
	{
		StartCoroutine(CoFadeOut(fadeTime));
		StopCoroutine(CoFadeOut(fadeTime));
	}
	public void FadeInOut(float fadeTime, Action callback)
	{
		StartCoroutine(CoFadeIn(fadeTime, true, callback));
	}
	public IEnumerator CoFadeIn(float fadeTime, bool mark, Action callback)
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
		if (mark)
        {
			callback();
			StartCoroutine(CoFadeOut(fadeTime));
		}
			
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