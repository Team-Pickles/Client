using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerInMultiPlayer : MonoBehaviour
{
    public static UIManagerInMultiPlayer instance;

    public GameObject tileMap;
    public GameObject loadingScene;

    private void Awake()
    {
        //Singleton ����
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists,destroying object!");
            Destroy(this);
        }

        StartCoroutine(ConnectToServer());
    }

    private IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(1);
        Client.instance.ConnectToServer();
        loadingScene.SetActive(false);
        tileMap.SetActive(true);

    }
}
