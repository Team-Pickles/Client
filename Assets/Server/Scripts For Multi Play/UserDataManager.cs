using UnityEngine;
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;
    public Socket socket;
    public string username = "Guest";
    public string accessToken;
    public string refreshToken;
    public bool isLogined = false;
    public string apiUrl { get; private set; }
    public bool isEditor = false;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists,destroying object!");
            Destroy(this);
        }
        //apiUrl = "http://localhost:3001/";
        apiUrl = "http://3.36.100.68:3001/";

        Reset();
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");
    }
    
    public void Reset() {
        username = "Guest";
        accessToken = "";
        refreshToken = "";
        isLogined = false;
    }

    public void SetAccessToken(string _accessToken)
    {
        accessToken = _accessToken;
    }

    public void SetToken(string _accessToken, string _refreshToken)
    {
        accessToken = _accessToken;
        refreshToken = _refreshToken;
    }

    public void SetUserName(string _username) {
        username = _username;
    }

    public void SetSocket()
    {
        string host = "ec2-3-36-114-195.ap-northeast-2.compute.amazonaws.com";

#if UNITY_EDITOR
        host = "127.0.0.1";
        isEditor = true;
#endif
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        //주소가 여러개일 수 있어서 배열로 받음
        IPAddress ipAddr = ipHost.AddressList[0];
        Debug.Log(ipAddr.ToString());

        //최종적인 주소
        IPEndPoint remoteendPoint = new IPEndPoint(ipHost.AddressList[0], 7777);

        try
        {
            socket = new Socket(remoteendPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = 10000;

            //연결 시도
            IAsyncResult result = socket.BeginConnect(remoteendPoint, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(1000, true);
            Debug.Log(result.AsyncState);
            if (success)
            {
                Debug.Log($"Connected To {socket.RemoteEndPoint.ToString()}");

                byte[] recvBuff = new byte[1024];
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Debug.Log($"[From Server] {recvData}");
            }
            else
            {
                // NOTE, MUST CLOSE THE SOCKET
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                throw new ApplicationException("Failed to connect server.");
            }
        }
        catch (Exception e)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Debug.Log(e.ToString());
            throw new ApplicationException("Failed to connect server.");

        }
    }
    //
    private void OnApplicationQuit() {
        if(UserDataManager.instance.isLogined) {
            string temp = $"Logout-";
            temp += UserDataManager.instance.accessToken;
            byte[] sendBuff = Encoding.UTF8.GetBytes(temp);
            int sendBytes = socket.Send(sendBuff);
            socket.Close();
        }
    }
}
