using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerInMultiPlayer : MonoBehaviour
{
    public static UIManagerInMultiPlayer instance;

    public GameObject tileMap;
    public GameObject loadingScene;
    public GameObject lobbyUI;
    public GameObject roomLobbyUI;
    public Button roomButtonPrefab;
    public Button MemberButtonPrefab;

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

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void setRoomList(Dictionary<string, string> roomDatas) {
        GameObject roomWrapper = null;
        for(int i = 0; i < lobbyUI.transform.childCount; ++i)
        {
            GameObject temp = lobbyUI.transform.GetChild(i).gameObject;
            if(temp.name == "RoomWrapper")
            {
                roomWrapper = temp;
                while(roomWrapper.transform.childCount > 0)
                {
                    DestroyImmediate(roomWrapper.transform.GetChild(0).gameObject);
                }
            }
        }

        if(roomWrapper == null)
        {
            Debug.Log("There is no RoomWrapper");
            return;
        }

        foreach(KeyValuePair<string, string> _roomData in roomDatas)
        {
            Button _btn = Instantiate<Button>(roomButtonPrefab, roomWrapper.transform);
            _btn.name = _roomData.Key;
            _btn.GetComponentInChildren<Text>().text = _roomData.Value;
        }
    }

    public void CreateRoomButtonClicked(InputField _roomName)
    {
        // roomLobbyUI.SetActive(true);
        lobbyUI.SetActive(false);

        ClientSend.CreateRoom(_roomName.text);

        _roomName.text = "";
    }

    public void StartGame()
    {
        
    }

    public void RefreshRoomList()
    {
        ClientSend.RoomList();
    }
}
