using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class UIManagerInMultiPlayer : MonoBehaviour
{
    public static UIManagerInMultiPlayer instance;

    public GameObject tileMap;
    public GameObject loadingScene;
    public GameObject lobbyUI;
    public GameObject roomLobbyUI;
    public Button roomButtonPrefab;
    public Button MemberButtonPrefab;

    public static Socket socket;

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

        string host = "127.0.0.1";
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        //주소가 여러개일 수 있어서 배열로 받음
        IPAddress ipAddr = ipHost.AddressList[0];
        //최종적인 주소
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        try
        {
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //연결 시도
            socket.Connect(endPoint);
            Debug.Log($"Connected To {socket.RemoteEndPoint.ToString()}");

            byte[] recvBuff = new byte[1024];
            int recvBytes = socket.Receive(recvBuff);
            string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
            string[] split = recvData.Split(',');
            Debug.Log($"[From Server] {split[0]}");
            Dictionary<int, string> rooms = new Dictionary<int, string>();
            for (int i=1; i< split.Length; i++)
            {
                rooms.Add(i, split[i]);
            }

            setRoomList(rooms);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }

       
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void setRoomList(Dictionary<int, string> roomDatas) {
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

        foreach(KeyValuePair<int, string> _roomData in roomDatas)
        {
            Button _btn = Instantiate<Button>(roomButtonPrefab, roomWrapper.transform);
            _btn.name = _roomData.Key.ToString();
            _btn.GetComponentInChildren<Text>().text = _roomData.Value;
        }

        loadingScene.SetActive(false);
    }

    public void CreateRoomButtonClicked(InputField _roomName)
    {
        Debug.Log("[CreateRoomButtonClicked]");
        string temp = $"CreateRoom {_roomName.text}";
        Debug.Log(temp);
        byte[] sendBuff = Encoding.UTF8.GetBytes(temp);
        Debug.Log(socket);
        int sendBytes = socket.Send(sendBuff);
        
        for(int errCnt = 0; errCnt < 5; ++errCnt){
            byte[] recvBuff = new byte[1024];
            try {
                int recvBytes = socket.Receive(recvBuff);
                string[] recvDatas = Encoding.UTF8.GetString(recvBuff, 0, recvBytes).Split(' ');
                Debug.Log($"[Room] {recvDatas[0]} {recvDatas[1]}");
                JoinClicked(recvDatas[0]);
                break;
                // roomLobbyUI.SetActive(true);
            } catch(SocketException _e) {
                if(_e.SocketErrorCode.ToString() != "TimedOut")
                {
                    Debug.Log(_e.SocketErrorCode);
                    break;
                } else {
                    Debug.Log(errCnt);
                }
            }
        }
    }

    public void StartGame()
    {
        
    }

    public void RefreshRoomList()
    {
        Dictionary<int, string> rooms = new Dictionary<int, string>();
        byte[] sendBuff = Encoding.UTF8.GetBytes($"RefreshRoom");
        int sendBytes = socket.Send(sendBuff);

        byte[] recvBuff = new byte[1024];
        int recvBytes = socket.Receive(recvBuff);
        string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);

        string[] split = recvData.Split(',');
        for (int i=1; i< split.Length; i++)
        {

            rooms.Add(i, split[i - 1]);
        }

        setRoomList(rooms);
    }

    public void JoinClicked(string _roomId)
    {
        string tempString = "JoinRoom" + _roomId;
        byte[] sendBuff = Encoding.UTF8.GetBytes(tempString);
        int sendBytes = socket.Send(sendBuff);

        byte[] recvBuff = new byte[1024];
        int recvBytes = socket.Receive(recvBuff);
        string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
        int port = int.Parse(recvData);

        if(port == -1)
        {
            RefreshRoomList();
            Debug.Log($"This room({_roomId}) is destroyed.");
        } else {
            StartCoroutine(ConnectToServer(port, _roomId));
        }
    }

    private IEnumerator ConnectToServer(int port, string _roomId)
    {
        yield return new WaitForSeconds(1);
        Client.instance.port = port;
        Client.instance.roomId = _roomId;
        Client.instance.ConnectToServer();
        lobbyUI.SetActive(false);
        loadingScene.SetActive(false);
        tileMap.SetActive(true);
    }
}
