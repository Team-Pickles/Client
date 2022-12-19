using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int id;
    private Color _tintColor = new Color(1.0f, 0.5f, 0.5f, 1.0f);

    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Collide()
    {
        GameManagerInServer.items.Remove(id);
        Destroy(gameObject);
    }

    public void SpringColorChange()
    {
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        float time = 0.0f;
        while (time <= 0.5f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(_tintColor, Color.white, time * 2.0f);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

}
