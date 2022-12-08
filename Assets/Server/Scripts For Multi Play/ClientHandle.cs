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

    public static void JoinRoomDone(Packet _packet)
    {
        int _mapId = _packet.ReadInt();
        string _members = _packet.ReadString();
        UIManagerInMultiPlayer.instance.RoomNameSetup(_packet.ReadString());
        string[] _memberList = _members.Split(',');
        UIManagerInMultiPlayer.instance.memberNames.Clear();
        for(int i = 0; i < _memberList.Length - 1; i++)
        {
            UIManagerInMultiPlayer.instance.memberNames.Add(_memberList[i]);
            UIManagerInMultiPlayer.instance.SetMemberItem(i, _memberList[i]);
        }
        MapListItem _item = UIManagerInMultiPlayer.instance.mapListItems[_mapId];
        UIManagerInMultiPlayer.instance.RoomInfoUiTexts[0].text = $"{_mapId}";
        UIManagerInMultiPlayer.instance.RoomInfoUiTexts[1].text = _item.map_tag;
        UIManagerInMultiPlayer.instance.RoomInfoUiTexts[2].text = _item.map_maker;
        UIManagerInMultiPlayer.instance.RoomInfoUiTexts[3].text = $"{_item.map_difficulty}";
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

    public static void PlayerDamaged(Packet _packet)
    {
        int _playerId = _packet.ReadInt();
        int _health = _packet.ReadInt();
        Debug.Log(_health);
        GameManagerInServer.players[_playerId].setHealth(_health);
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
        if(GameManagerInServer.players.Count != 0) {
            Destroy(GameManagerInServer.players[_id].gameObject);
            GameManagerInServer.players.Remove(_id);
        }
        string _members = _packet.ReadString();
        string[] _memberList = _members.Split(',');
        UIManagerInMultiPlayer.instance.MemberListUIOff();
        UIManagerInMultiPlayer.instance.memberNames.Clear();
        for(int i = 0; i < _memberList.Length - 1; i++)
        {
            UIManagerInMultiPlayer.instance.memberNames.Add(_memberList[i]);
            UIManagerInMultiPlayer.instance.SetMemberItem(i, _memberList[i]);
        }
        Debug.Log(_id + " " + _members);
    }

    public static void spawnProjectile(Packet _packet)
    {
        int _projectileID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _thrownByPlayer = _packet.ReadInt();
        int _projectileCount = _packet.ReadInt();

        GameManagerInServer.instance.SpawnProjectile(_projectileID, _position, _thrownByPlayer);
        GameManagerInServer.players[_thrownByPlayer].GrenadeCount = _projectileCount;
    }

    public static void projectilePosition(Packet _packet)
    {
        int _projectileID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManagerInServer.projectiles.TryGetValue(_projectileID, out ProjectileManager _projectile))
        {
            _projectile.transform.localPosition = _position;
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
            _explodedPositions.Add(new Vector3Int((int)now.x, (int)now.y, (int)now.z));
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
        int _bulletCount = _packet.ReadInt();

        GameManagerInServer.instance.SpawnBullet(_bulletID, _position,_thrownByPlayer);
        GameManagerInServer.players[_thrownByPlayer].BulletCount = _bulletCount;
    }

    public static void BulletPosition(Packet _packet)
    {
        int _bulletID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManagerInServer.bullets.TryGetValue(_bulletID, out BulletManager _bullet))
        {
            if(_bullet != null)
                _bullet.transform.localPosition = _position;
        }
    }

    public static void BulletCollide(Packet _packet)
    {
        
        int _bulletID = _packet.ReadInt();
        if (GameManagerInServer.bullets.TryGetValue(_bulletID, out BulletManager _bullet))
        {
            if(_bullet != null)
                _bullet.Collide();
        }
    }

    public static void SpawnItem(Packet _packet)
    {
        int _itemID = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _itemType = _packet.ReadInt();

        GameManagerInServer.instance.SpawnItem(_itemID, _position, _itemType);
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

    public static void ItemPickedUp(Packet _packet)
    {
        int _itemType = _packet.ReadInt();
        int _itemCnt = _packet.ReadInt();
        int _id = _packet.ReadInt();

        if(_itemType == (int)TileType.trash)
        {
            GameManagerInServer.players[_id].BulletCount = _itemCnt;
        }
        else if(_itemType == (int)TileType.grenade2)
        {
            GameManagerInServer.players[_id].GrenadeCount = _itemCnt;
        }
    }
    
    //
    public static void CharactorFlip(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _isFlip = _packet.ReadBool();

        if (GameManagerInServer.players.ContainsKey(_id))
            GameManagerInServer.players[_id].GetComponent<SpriteRenderer>().flipX = _isFlip;
    }

    public static void RopeACK(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _onRope = _packet.ReadBool();

        if (GameManagerInServer.players.ContainsKey(_id))
        {
            GameManagerInServer.players[_id].onRope = _onRope;
            if (_onRope)
                GameManagerInServer.players[_id].GetComponent<Animator>().SetBool("isHanging", true);
            else
                GameManagerInServer.players[_id].GetComponent<Animator>().SetBool("isHanging", false);
        }
            
    }


    public static void SpawnEnemy(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        Vector3 _enemyPos = _packet.ReadVector3();
        GameManagerInServer.instance.SpawnEnemy(_enemyId, _enemyPos);
    }

    public static void EnemyPosition(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        Vector3 _enemyPos = _packet.ReadVector3();
        bool isMove = _packet.ReadBool();

        if (GameManagerInServer.enemies.TryGetValue(_enemyId, out ServerEnemy _enemy))
        {
            _enemy.transform.position = _enemyPos;
            _enemy._animator.SetBool("isMoving", isMove);
        }
    }

    public static void EnemyHit(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();

        if (GameManagerInServer.enemies.TryGetValue(_enemyId, out ServerEnemy _enemy))
        {
            _enemy.EnemyHit();
        }
    }


    public static void StartGame(Packet _packet)
    {
        int _mapId = _packet.ReadInt();
        UIManagerInMultiPlayer.instance.StartGameProcess(_mapId);
    }

    public static void allSpawned(Packet _packet)
    {
        bool _done = _packet.ReadBool();
        UIManagerInMultiPlayer.instance.SetPlayerInfo();
    }
}
