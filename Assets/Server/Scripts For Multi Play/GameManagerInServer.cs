using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManagerInServer : MonoBehaviour
{
    public static GameManagerInServer instance;

    //�÷��̾�� �Ŵ��� �ֱ� ����
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ProjectileManager> projectiles = new Dictionary<int, ProjectileManager>();
    public static Dictionary<int, BulletManager> bullets = new Dictionary<int, BulletManager>();
    public static Dictionary<int, ItemManager> items = new Dictionary<int, ItemManager>();
    public static Dictionary<int, ServerEnemy> enemies = new Dictionary<int, ServerEnemy>();
    public static Dictionary<int, DoorAction> doors = new Dictionary<int, DoorAction>();
    public static Dictionary<int, Camera> playerCamera = new Dictionary<int, Camera>();
    public Tilemap Tilemap; 


    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject itemSpawnerPrefab;
    public GameObject projectilePrefab;
    public GameObject enemyPrefab;
    public GameObject bulletPrefab;
    public GameObject[] itemPrefabs;
    public GameObject[] doorPrefabs;

    public List<GameObject> allObjects = new List<GameObject>();
    public int nowCamId;
    public bool isStoppedByEsc = false;

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

    public void Update()
    {
        if(Client.instance.myId != 0 && players.Count != 0)
        {
            PlayerManager _player;
            if(players.TryGetValue(Client.instance.myId, out _player) && players.TryGetValue(nowCamId, out _player))
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    if(players[Client.instance.myId].health <= 0 && players[nowCamId].health > 0)
                    {
                        UpdateMyCamera(nowCamId);
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(isStoppedByEsc)
                {
                    isStoppedByEsc = false;
                    UIManagerInMultiPlayer.instance.NoForRestart(true);
                }
                else
                {
                    isStoppedByEsc = true;
                    UIManagerInMultiPlayer.instance.AskToRestartByKey();
                }
            }
        }
    }

    public void UpdateMyCamera(int camKey)
    {
        foreach(KeyValuePair<int, Camera> _camPair in GameManagerInServer.playerCamera)
        {
            if(_camPair.Key == camKey || GameManagerInServer.players[_camPair.Key].health <= 0)
                continue;
            GameManagerInServer.playerCamera[camKey].enabled = false;
            _camPair.Value.enabled = true;
            nowCamId = _camPair.Key;
            break;
        }
        Debug.Log(nowCamId);
    }

    public void SpawnPlayer(int _id, string _username, Vector2 _position, Quaternion _rotaion)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotaion);
            nowCamId = _id;
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotaion);
        }
        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        playerCamera.Add(_id, _player.GetComponentInChildren<Camera>());
        players.Add(_id, _player.GetComponent<PlayerManager>());
        allObjects.Add(_player);
    }

    public void SpawnProjectile(int _projectileId, Vector3 _position, int _thrownByPlayer)
    {
        GameObject _projectile = Instantiate(projectilePrefab, players[_thrownByPlayer].transform);
        _projectile.transform.localPosition = _position;
        _projectile.GetComponent<ProjectileManager>().Initialize(_projectileId);
        projectiles.Add(_projectileId, _projectile.GetComponent<ProjectileManager>());
    }

    public void SpawnBullet(int _bulletId, Vector3 _position, int _thrownByPlayer)
    {
        //GameObject _bullet = Instantiate(bulletPrefab, _position, Quaternion.identity);
        GameObject _bullet = Instantiate(bulletPrefab, players[_thrownByPlayer].transform);
        _bullet.transform.localPosition = _position;
        _bullet.GetComponent<BulletManager>().Initialize(_bulletId);
        bullets.Add(_bulletId, _bullet.GetComponent<BulletManager>());
    }

    public void SpawnItem(int _itemId, Vector3 _position, int _itemType)
    {
        Debug.Log(_itemType);
        GameObject _item = Instantiate(itemPrefabs[_itemType - (int)TileType.Item - 1], _position, Quaternion.identity);
        ItemManager _itemManager;
        _item.TryGetComponent<ItemManager>(out _itemManager);
        if(_itemManager != null)
        {
            _itemManager.Initialize(_itemId);
            items.Add(_itemId, _itemManager);
        }
        allObjects.Add(_item);
    }

    public void SpawnEnemy(int _enemyId, Vector3 _position)
    {
        GameObject _enemy = Instantiate(enemyPrefab, _position, Quaternion.identity);
        _enemy.GetComponent<ServerEnemy>().Initialize(_enemyId);
        enemies.Add(_enemyId, _enemy.GetComponent<ServerEnemy>());
        allObjects.Add(_enemy);
    }

    public void SpawnDoor(int _doorId, Vector3 _position, bool _isIndoor)
    {
        int prefabId = _isIndoor ? 0 : 1;
        
        GameObject _door = Instantiate(doorPrefabs[prefabId], _position, Quaternion.identity);
        _door.GetComponent<DoorAction>().Initialize(_doorId, _isIndoor);
        doors.Add(_doorId, _door.GetComponent<DoorAction>());
        allObjects.Add(_door);
    }
}
