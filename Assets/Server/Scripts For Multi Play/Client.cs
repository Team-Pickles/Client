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
    public int port;
    public int myId = 0;
    public string roomId;
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

    //���ø����̼��� ����Ǳ� ���� ȣ��
    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void RoomExit()
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        tcp = new TCP();
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
            { (int)ServerPackets.roomJoined, ClientHandle.JoinRoomDone },
            
            { (int)ServerPackets.charactorFlip, ClientHandle.CharactorFlip },
            { (int)ServerPackets.ropeACK, ClientHandle.RopeACK },
            { (int)ServerPackets.playerHealth, ClientHandle.PlayerDamaged},
            { (int)ServerPackets.spawnEnemy, ClientHandle.SpawnEnemy },
            { (int)ServerPackets.enemyPosition, ClientHandle.EnemyPosition },
            { (int)ServerPackets.enemyHit, ClientHandle.EnemyHit },
            { (int)ServerPackets.startGame, ClientHandle.StartGame},
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
            //BeginConnect�� �񵿱��, �ٸ� �����忡�� ���� ���� �� �Ϸ�Ǹ� �˸�
            //���� ȣ�� �����带 �������� ����
            socket.BeginConnect(instance.ip, instance.port, ConnenctionCallback, socket);
        }

        private void ConnenctionCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
                return;

            //data �ۼ��� ������ NetworkStream ��ȯ
            stream = socket.GetStream();
            receiveData = new Packet();

            //BeginRead �񵿱��, �ٸ� �����忡�� ���� ���� �� �Ϸ�Ǹ� �˸�
            //���� ȣ�� �����带 �������� ����
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                //NetworkStream���� ���� ����Ʈ �� ��ȯ
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
                //receiveData�� �� ó�� �κп��� data�� ���̰� int������ ��
                //send�ÿ� data�� ���̸� ������ �� �տ� ����ϱ� ����
                _packetLength = receiveData.ReadInt();
                if (_packetLength <= 0)
                    return true;
            }

            while (_packetLength > 0 && _packetLength <= receiveData.UnreadLength())
            {

                byte[] _packetBytes = receiveData.ReadBytes(_packetLength);
                //������ �����忡�� ��Ŷ�� �´� �Լ� ����
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    //stream���� �о�� �����͸� ��Ŷ ���·� �����.
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                //stream�� �������� ��Ŷ�� ���� �� �ֱ⿡.
                _packetLength = 0;
                if (receiveData.UnreadLength() >= 4)
                {
                    _packetLength = receiveData.ReadInt();
                    if (_packetLength <= 0)
                        return true;
                }
            }

            //while ���� ������ ������ > 0 �̱� ������
            // <= 0�� true��
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
                //datagram data�� ������ ����Ʈ���� ��ȯ
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

            //��Ŷó���� ������ �����忡��
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
