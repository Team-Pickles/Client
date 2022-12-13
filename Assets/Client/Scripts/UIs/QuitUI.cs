using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitUI : MonoBehaviour
{
    public GameObject AskExitUi;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(AskExitUi.activeSelf)
            {
                NoButtonClicked();
            }  
            else
            {
                AskExitUi.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    public void NoButtonClicked()
    {
        AskExitUi.SetActive(false);
        Time.timeScale = 1;
    }

    public void YesButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
