using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }


    public static void TCPConnenctinCheckReceived()
    {
        using(Packet _packet = new Packet((int)ClientPackets.TCPConnenctinCheckReceived))
        {
            _packet.Write(Client.instance.myId);
            //_packet.Write(UIManager.instance.usernameField.text);
            _packet.Write("test");
            _packet.Write(Client.instance.roomId);
            Debug.Log("TCPConnenctinCheckReceived");
            SendTCPData(_packet);
        }
    }

    public static void UDPTestReceive()
    {
        using (Packet _packet = new Packet((int)ClientPackets.udpTestReceive))
        {
            _packet.Write("Received a UDP packet");

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
                _packet.Write(_input);

            _packet.Write(GameManagerInServer.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerThrowGrenade(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerThrowItem))
        {
            _packet.Write(_facing);
            SendTCPData(_packet);
        }
    }

    public static void PlayerShoot(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(_facing);
            SendTCPData(_packet);
        }
    }

    public static void PlayerStartVaccume(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerStartVacuume))
        {
            _packet.Write(_facing);
            SendTCPData(_packet);
        }
    }

    public static void PlayerEndVaccume()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerEndVacuume))
        {
            SendTCPData(_packet);
        }
    }


    public static void ItemCollide(int _itemId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.ItemCollide))
        {
            _packet.Write(_itemId);
            SendTCPData(_packet);
        }
    }

    public static void CreateRoom(string _roomName)
    {
        using (Packet _packet = new Packet((int)ClientPackets.createRoom))
        {
            _packet.Write(_roomName);
            SendTCPData(_packet);
        }
    }

    public static void JoinRoom(string _roomId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.joinRoom))
        {
            _packet.Write(_roomId);
            SendTCPData(_packet);
        }
    }

    public static void RoomList()
    {
        using (Packet _packet = new Packet((int)ClientPackets.roomList))
        {
            SendTCPData(_packet);
        }
    }
}
