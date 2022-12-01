using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;

[Serializable]
public class MapListItem
{
    public int map_id;
    public string map_tag;
    public string map_info;
    public int map_grade;
    public int map_difficulty;
    public string map_maker;
}

public class UIManagerInMultiPlayer : MonoBehaviour
{
    public static UIManagerInMultiPlayer instance;


    public GameObject loadingScene;
    public GameObject lobbyUI;
    public GameObject roomLobbyUI;
    public List<Text> RoomInfoUiTexts;
    public List<GameObject> MemberListUiTexts;
    public GameObject RoomCreatePopup;
    public List<GameObject> MapListUis;
    public Button MapLeftPageButton;
    public Button MapRightPageButton;
    public Button roomButtonPrefab;
    public List<Text> SelectedMapInfos;
    public Text RoomNameText;

    public static Socket socket;
    public List<string> memberNames = new List<string>();
    public Dictionary<int, MapListItem> mapListItems = new Dictionary<int, MapListItem>();

    private int nowMapItemStartNum = 0;
    private string roomName;
    private int selectedMapId;
    private string defaultMapJson;

    private void Awake()
    {
        //Singleton 패턴
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists,destroying object!");
            Destroy(this);
        }
        string path = "MapData/MyMap.json";
        if(File.Exists(path) == false){
            Debug.LogError("Load failed. There is no file(MyMap.json).");
            return;
        }
        defaultMapJson = File.ReadAllText(path);
        setDefaultMapInfo();

        string host = "127.0.0.1";
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        //주소가 여러개일 수 있어서 배열로 받음
        IPAddress ipAddr = ipHost.AddressList[0];
        //최종적인 주소
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        try
        {
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = 10000;
            //연결 시도
            IAsyncResult result = socket.BeginConnect(endPoint, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(1000, true);

            if (success)
            {
                Debug.Log($"Connected To {socket.RemoteEndPoint.ToString()}");

                byte[] recvBuff = new byte[1024];
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                string[] split = recvData.Split(',');
                Debug.Log($"[From Server] {split[0]}");
                Dictionary<int, string> rooms = new Dictionary<int, string>();
                for (int i = 1; i < split.Length; i++)
                {
                    rooms.Add(i, split[i]);
                }

                setRoomList(rooms);
            }
            else
            {
                // NOTE, MUST CLOSE THE SOCKET

                socket.Close();
                throw new ApplicationException("Failed to connect server.");
            }

        }
        catch (Exception e)
        {
            socket.Close();
            Debug.Log(e.ToString());
            throw new ApplicationException("Failed to connect server.");

        }


    }

    public int TCPConnectTimeo(string hostname, string service, string nsec){return 1; }


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
            _btn.name = _roomData.Key.ToString() + "_" + _roomData.Value.Split("-")[0];
            _btn.GetComponentInChildren<Text>().text = _roomData.Value.Split("-")[1];
        }

        loadingScene.SetActive(false);
    }

    public void CreateRoomButtonClicked(InputField _roomName)
    {
        Debug.Log("[CreateRoomButtonClicked]");
        roomName = _roomName.text;
        string temp = $"RoomNameCheck-{roomName}";
        Debug.Log(temp);
        byte[] sendBuff = Encoding.UTF8.GetBytes(temp);
        Debug.Log(socket);
        int sendBytes = socket.Send(sendBuff);
        socket.ReceiveTimeout = 3000;
        for(int errCnt = 0; errCnt < 5; ++errCnt){
            byte[] recvBuff = new byte[1024];
            try {
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Debug.Log($"[RoomNameIsDuplicated] {recvData}");
                if (recvData == "Ok")
                {
                    RoomCreatePopup.SetActive(true);
                    selectedMapId = 0;
                    SelectedMapInfos[0].text = $"{selectedMapId}";
                    SelectedMapInfos[1].text = mapListItems[selectedMapId].map_tag;
                    SelectedMapInfos[2].text = $"{mapListItems[selectedMapId].map_maker}";
                    AllMapListLoad();
                }
                break;

            } catch(SocketException _e) {
                if(_e.SocketErrorCode.ToString() != "TimedOut")
                {
                    Debug.Log(_e.SocketErrorCode);
                    break;
                } else {
                    Debug.Log(_e.SocketErrorCode + "_" + errCnt);
                }
            }
        }
    }

    public void CreateRoomSubmitButtonClicked()
    {
        Debug.Log("[CreateRoomSubmitButtonClicked]");
        string temp = $"CreateRoom-{roomName}-{SelectedMapInfos[0].text}";
        Debug.Log(temp);
        byte[] sendBuff = Encoding.UTF8.GetBytes(temp);
        Debug.Log(socket);
        int sendBytes = socket.Send(sendBuff);
        socket.ReceiveTimeout = 3000;
        for(int errCnt = 0; errCnt < 5; ++errCnt){
            byte[] recvBuff = new byte[1024];
            try {
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Debug.Log($"[Room] {recvData}");
                if (recvData != "None")
                {
                    string[] recvDatas = recvData.Split('-');
                    Debug.Log($"[Room] {recvDatas[0]} {recvDatas[1]}");
                    JoinClicked(recvDatas[0]);
                    lobbyUI.SetActive(false);

                }
                break;

            } catch(SocketException _e) {
                if(_e.SocketErrorCode.ToString() != "TimedOut")
                {
                    Debug.Log(_e.SocketErrorCode);
                    break;
                } else {
                    Debug.Log(_e.SocketErrorCode + "_" + errCnt);
                }
            }
        }
    }

    public void MapSearchButtonClicked(InputField _mapTagField)
    {
        string _mapTag = _mapTagField.text;
        if(_mapTag == ""){
            AllMapListLoad();
            return;
        }
        MapListLoadWithMapTag(_mapTag);
    }

    public void AllMapListLoad()
    {

        mapListItems.Clear();
        setDefaultMapInfo();
        nowMapItemStartNum = 0;
        MapLeftPageButton.interactable = false;
        StartCoroutine(IneternetConnectCheck(isConnected => {
            if (isConnected)
            {
                Debug.Log("Server Available!");
                StartCoroutine(GetMapList(mapListItemCnt => {
                    MapListSet();
                    if(mapListItemCnt < 5)
                        MapRightPageButton.interactable = false;
                    else
                        MapRightPageButton.interactable = true;
                }, "http://localhost:3001/api/map/getAllList"));
            }
            else
            {
                Debug.Log("Internet or server Not Available");
            }
        }));
    }

    public void MapListLoadWithMapTag(string _mapTag)
    {

        mapListItems.Clear();
        setDefaultMapInfo();
        nowMapItemStartNum = 0;
        MapLeftPageButton.interactable = false;
        StartCoroutine(IneternetConnectCheck(isConnected => {
            if (isConnected)
            {
                Debug.Log("Server Available!");
                StartCoroutine(GetMapList(mapListItemCnt => {
                    MapListSet();
                    if(mapListItemCnt < 5)
                        MapRightPageButton.interactable = false;
                    else
                        MapRightPageButton.interactable = true;
                }, "http://localhost:3001/api/map/getListByTag/" + _mapTag));
            }
            else
            {
                Debug.Log("Internet or server Not Available");
            }
        }));
    }

    public void MapLeftPageButtonClicked()
    {
        nowMapItemStartNum -= 5;
        if(nowMapItemStartNum == 0)
            MapLeftPageButton.interactable = false;
        MapListSet();
        MapRightPageButton.interactable = true;
    }

    public void MapRightPageButtonClicked()
    {
        nowMapItemStartNum += 5;
        if(nowMapItemStartNum + 5 >= mapListItems.Count)
            MapRightPageButton.interactable = false;
        MapListSet();
        MapLeftPageButton.interactable = true;
    }

    IEnumerator GetMapList(Action<int> ResultHandler, string _url){
        using ( UnityWebRequest request = UnityWebRequest.Get(_url))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();
            string _result = request.downloadHandler.text;
            string _forparse = "{\"Items\":" + _result + "}";
            MapDatas mapInfos = JsonUtility.FromJson<MapDatas>(_forparse);
            foreach(MapDataClass _mapInfo in mapInfos.Items)
            {
                MapListItem _item = new MapListItem();
                _item.map_id = _mapInfo.map_id;
                _item.map_tag = _mapInfo.map_tag;
                _item.map_info = _mapInfo.map_info;
                _item.map_grade = _mapInfo.map_grade;
                _item.map_difficulty = _mapInfo.map_difficulty;
                _item.map_maker = _mapInfo.map_maker;
                mapListItems.Add(_item.map_id, _item);
            }

            if (request.error != null)
            {
                Debug.Log(request.error);
            }
            else
            {
                ResultHandler(mapListItems.Count);
            }
            request.downloadHandler.Dispose();
            request.Dispose();
        }
    }

    IEnumerator IneternetConnectCheck(Action<bool> action)
    {
        using (UnityWebRequest request = new UnityWebRequest("http://localhost:3001/"))
        {

            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.error != null)
            {
                action(false);
            }
            else
            {
                action(true);
            }
            request.downloadHandler.Dispose();
            request.Dispose();
        }
    }

    private void MapListUIOff()
    {
        foreach(GameObject MapUI in MapListUis)
        {
            MapUI.SetActive(false);
        }
    }

    public void MemberListUIOff()
    {
        foreach(GameObject memberUI in MemberListUiTexts)
        {
            memberUI.SetActive(false);
        }
    }

    private void MapListSet()
    {
        MapListUIOff();
        List<MapListItem> mapListItemsValues = new List<MapListItem>(mapListItems.Values);
        for(int nowMapItemNum = nowMapItemStartNum; nowMapItemNum < mapListItemsValues.Count; nowMapItemNum++)
        {
            GameObject nowListItem = MapListUis[nowMapItemNum-nowMapItemStartNum];
            MapListItem _item = mapListItemsValues[nowMapItemNum];
            Debug.Log(_item.map_id);
            nowListItem.SetActive(true);
            nowListItem.transform.GetChild(0).GetComponent<Text>().text = $"{_item.map_id}";
            nowListItem.transform.GetChild(1).GetComponent<Text>().text = _item.map_tag;
            nowListItem.transform.GetChild(2).GetComponent<Text>().text = $"{_item.map_grade}";
            nowListItem.transform.GetChild(3).GetComponent<Text>().text = $"{_item.map_difficulty}";
            nowListItem.transform.GetChild(4).GetComponent<Text>().text = $"{_item.map_maker}";
            if(nowMapItemNum-nowMapItemStartNum == 4)
            {
                break;
            }
        }
    }

    public void MapItemClicked(Text _mapIdText)
    {
        int _mapId = Convert.ToInt32(_mapIdText.text);
        MapListItem _nowMap = mapListItems[_mapId];
        selectedMapId = _nowMap.map_id;
        SelectedMapInfos[0].text = $"{_nowMap.map_id}";
        SelectedMapInfos[1].text = _nowMap.map_tag;
        SelectedMapInfos[2].text = $"{_nowMap.map_maker}";
    }

    public void ReadyToStartGame()
    {
        //
        int mapId = Convert.ToInt32(RoomInfoUiTexts[0].text);
        ClientSend.ReadyToStartGame(Client.instance.roomId);
        loadingScene.SetActive(true);
    }

    public void StartGameProcess(int map_id)
    {
        loadingScene.SetActive(false);
        roomLobbyUI.SetActive(false);
        MapDataLoader.instance.Load(mapListItems[map_id].map_info);
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

    public void SetMemberItem(int _id, string _username)
    {
        MemberListUiTexts[_id].SetActive(true);
        MemberListUiTexts[_id].GetComponentInChildren<Text>().text = _username;
    }

    public void ExitRoomButtonClicked()
    {
        Client.instance.RoomExit();
        roomLobbyUI.SetActive(false);
        loadingScene.SetActive(true);
        BackToMain();
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
            mapListItems.Clear();
            setDefaultMapInfo();
            loadingScene.SetActive(true);
            StartCoroutine(IneternetConnectCheck(isConnected => {
                if (isConnected)
                {
                    Debug.Log("Server Available!");
                    StartCoroutine(GetMapList(mapListItemCnt => {
                    }, "http://localhost:3001/api/map/getAllList"));
                }
                else
                {
                    Debug.Log("Internet or server Not Available");
                }
            }));
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
        roomLobbyUI.SetActive(true);
    }

    public void RoomNameSetup(string _roomName)
    {
        roomName = _roomName;
        RoomNameText.text = roomName;
    }

    private void setDefaultMapInfo()
    {
        MapListItem _temp = new MapListItem();
        _temp.map_id = 0;
        _temp.map_maker = "pickles";
        _temp.map_tag = "defaultMap";
        _temp.map_info = defaultMapJson;
        _temp.map_difficulty = 0;
        _temp.map_grade = 0;
        mapListItems.Add(0, _temp);
    }
}
