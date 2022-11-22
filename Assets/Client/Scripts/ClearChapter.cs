using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearChapter : MonoBehaviour
{
    bool istouched = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
            {
                istouched = true;
                break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "player":
            {
                istouched = false;
                break;
            }
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (istouched && Input.GetKeyDown(KeyCode.UpArrow))
        {
            SceneManager.LoadScene("LevelChange");
            GameObject.Find("Player").transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
