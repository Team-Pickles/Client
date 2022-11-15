using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ClientHandle : MonoBehaviour
{
    public static void TCPConnenctinCheck(Packet _packet)
    {
        // sting ,int 
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"msg from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.TCPConnenctinCheckReceived();

        Debug.Log("Trying connect to server");
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    
    }

    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();
        Debug.Log($"Message from udp server: {_msg}");
        ClientSend.UDPTestReceive();
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector2 _position = _packet.ReadVector2();
        Quaternion _rotation = _packet.ReadQuaternion();

        Debug.Log(_id);
        Debug.Log(_username);
        Debug.Log(_position);
        Debug.Log(_rotation);

        Debug.Log("Spawn Player");
        GameManagerInServer.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManagerInServer.players.ContainsKey(_id))
            GameManagerInServer.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManagerInServer.players.ContainsKey(_id))
            GameManagerInServer.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManagerInServer.players[_id].gameObject);
        GameManagerInServer.players.Remove(_id);

    }

    public static void spawnProjectile(Packet _packet)
    {
        int _projectileID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _thrownByPlayer = _packet.ReadInt();

        GameManagerInServer.instance.SpawnProjectile(_projectileID, _position);
        //GameManagerInServer.players[_thrownByPlayer].itemCount--;
    }

    public static void projectilePosition(Packet _packet)
    {
        int _projectileID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManagerInServer.projectiles.TryGetValue(_projectileID, out ProjectileManager _projectile))
        {
            _projectile.transform.position = _position;
        }
    }

    public static void projectileExploded(Packet _packet)
    {
        int _projectileID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _arrLen = _packet.ReadInt();
        List<Vector3Int> _explodedPositions = new List<Vector3Int>();
        for(int i = 0; i < _arrLen; ++i) {
            Vector3 now = _packet.ReadVector3();
            if(now.x < 0) {
                _explodedPositions.Add(new Vector3Int((int)now.x - 1, (int)now.y - 1, (int)now.z));
            }
            else {
                _explodedPositions.Add(new Vector3Int((int)now.x, (int)now.y - 1, (int)now.z));
            }
        }
        if (GameManagerInServer.projectiles.TryGetValue(_projectileID, out ProjectileManager _projectile))
        {
            _projectile.Explode(_position, _explodedPositions);
        }
    }

    public static void SpawnBullet(Packet _packet)
    {
        int _bulletID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _thrownByPlayer = _packet.ReadInt();

        GameManagerInServer.instance.SpawnBullet(_bulletID, _position);
        //GameManagerInServer.players[_thrownByPlayer].itemCount--;
    }

    public static void BulletPosition(Packet _packet)
    {
        int _bulletID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManagerInServer.bullets.TryGetValue(_bulletID, out BulletManager _bullet))
        {
            _bullet.transform.position = _position;
        }
    }

    public static void BulletCollide(Packet _packet)
    {
        
        int _bulletID = _packet.ReadInt();
        if (GameManagerInServer.bullets.TryGetValue(_bulletID, out BulletManager _bullet))
        {
            _bullet.Collide();
        }
    }

    public static void SpawnItem(Packet _packet)
    {
        int _itemID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManagerInServer.instance.SpawnItem(_itemID, _position);
        //GameManagerInServer.players[_thrownByPlayer].itemCount--;
    }

    public static void ItemPosition(Packet _packet)
    {
        int _itemID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManagerInServer.items.TryGetValue(_itemID, out ItemManager _item))
        {
            _item.transform.position = _position;
        }
    }

    public static void ItemCollide(Packet _packet)
    {
        int _itemID = _packet.ReadInt();

        if (GameManagerInServer.items.TryGetValue(_itemID, out ItemManager _item))
        {
            _item.Collide();
        }
    }

    public static void RoomCreated(Packet _packet)
    {
        string _roomId = _packet.ReadString();
        if(_roomId == "None")
        {
            Debug.Log("Room Name is duplicated");
        } else {
            Client.instance.roomId = _roomId;
        }
    }

    public static void RoomList(Packet _packet)
    {
        int roomCount = _packet.ReadInt();
        Dictionary<string, string> rooms = new Dictionary<string, string>();
        for(int i = 0; i < roomCount; ++i)
        {
            rooms.Add(_packet.ReadString(), _packet.ReadString());
        }
        UIManagerInMultiPlayer.instance.setRoomList(rooms);
    }
}
