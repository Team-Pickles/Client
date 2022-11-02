using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    public static Client instance;
    //4Byte
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    private bool isConnect = false;

    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists,destroying object!");
            Destroy(this);
        }
    }

    //어플리케이션이 종료되기 전에 호출
    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        Debug.Log("Test2");
        tcp = new TCP();
        Debug.Log("Test3");
        udp = new UDP();
        
        InitializeClientData();
        
        tcp.Connect();
        isConnect = true;
    }

    private void Disconnect()
    {
        if (isConnect)
        {
            isConnect = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnencted from server.");
        }
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.tcpConnenctinCheck, ClientHandle.TCPConnenctinCheck },
            { (int)ServerPackets.udpTest, ClientHandle.UDPTest },
            { (int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer },
            { (int)ServerPackets.playerPosition, ClientHandle.PlayerPosition},
            { (int)ServerPackets.playerRotation, ClientHandle.PlayerRotation },
            { (int)ServerPackets.playerDisconnected, ClientHandle.PlayerDisconnected },
            { (int)ServerPackets.spawnProjectile, ClientHandle.spawnProjectile },
            { (int)ServerPackets.projectilePosition, ClientHandle.projectilePosition },
            { (int)ServerPackets.projectileExploded, ClientHandle.projectileExploded },
            { (int)ServerPackets.spawnBullet, ClientHandle.SpawnBullet },
            { (int)ServerPackets.bulletPosition, ClientHandle.BulletPosition},
            { (int)ServerPackets.bulletCollide, ClientHandle.BulletCollide },
            { (int)ServerPackets.spawnItem, ClientHandle.SpawnItem},
            { (int)ServerPackets.ItemPosition, ClientHandle.ItemPosition },
            { (int)ServerPackets.itemCollide, ClientHandle.ItemCollide },
        };
        Debug.Log("Initiallized packets.");
    }
 
    public class TCP
    {
        public TcpClient socket;
        private NetworkStream stream;
        private byte[] receiveBuffer;
        private Packet receiveData;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize,
            };

            receiveBuffer = new byte[dataBufferSize];
            //BeginConnect는 비동기식, 다른 스레드에서 연결 수행 후 완료되면 알림
            //현재 호출 스레드를 차단하지 않음
            socket.BeginConnect(instance.ip, instance.port, ConnenctionCallback, socket);
        }

        private void ConnenctionCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
                return;

            //data 송수신 가능한 NetworkStream 반환
            stream = socket.GetStream();
            receiveData = new Packet();

            //BeginRead 비동기식, 다른 스레드에서 연결 수행 후 완료되면 알림
            //현재 호출 스레드를 차단하지 않음
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                //NetworkStream에서 읽은 바이트 수 반환
                int _byteLength = stream.EndRead(_result);
                if(_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receiveData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving TCP data: {_ex}");
                Disconnect();
            }
        }


        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;
            receiveData.SetBytes(_data);

            if (receiveData.UnreadLength() >= 4)
            {
                //receiveData의 맨 처음 부분에는 data의 길이가 int형으로 들어감
                //send시에 data의 길이를 버퍼의 맨 앞에 기록하기 때문
                _packetLength = receiveData.ReadInt();
                if (_packetLength <= 0)
                    return true;
            }

            while (_packetLength > 0 && _packetLength <= receiveData.UnreadLength())
            {

                byte[] _packetBytes = receiveData.ReadBytes(_packetLength);
                //별도의 스레드에서 패킷에 맞는 함수 실행
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    //stream에서 읽어온 데이터를 패킷 형태로 만든다.
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                //stream에 여러개의 패킷이 있을 수 있기에.
                _packetLength = 0;
                if (receiveData.UnreadLength() >= 4)
                {
                    _packetLength = receiveData.ReadInt();
                    if (_packetLength <= 0)
                        return true;
                }
            }

            //while 에서 나오는 조건이 > 0 이기 때문에
            // <= 0은 true임
            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        private void Disconnect()
        {
            instance.Disconnect();

            stream = null;
            receiveBuffer = null;
            receiveData = null;
            socket = null;
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
            }
            catch (Exception _ex)
            {
                Debug.LogError($"Error Sending data to Server via TCP:{_ex}");
            }
        }

    }


    public class UDP
    {

        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                //datagram data를 포함한 바이트열을 반환
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4)
                {
                    instance.Disconnect();
                    return;
                }

                HandleData(_data);
            }
            catch
            {
                Disconnect();
            }

        }

        private void HandleData(byte[] _data)
        {
            using(Packet _packet = new Packet(_data))
            {
                int _packtLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packtLength);
            }

            //패킷처리는 별도의 스레드에서
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");
            }
        }

        private void Disconnect()
        {
            instance.Disconnect();

            endPoint = null;
            socket = null;
        }

    }
}
