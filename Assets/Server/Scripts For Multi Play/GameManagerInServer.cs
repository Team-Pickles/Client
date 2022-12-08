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
    public Tilemap Tilemap; 


    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject itemSpawnerPrefab;
    public GameObject projectilePrefab;
    public GameObject enemyPrefab;
    public GameObject bulletPrefab;
    public GameObject[] itemPrefabs;
    public GameObject camera;

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

    public void SpawnPlayer(int _id, string _username, Vector2 _position, Quaternion _rotaion)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotaion);
            _player.GetComponent<PlayerMoveManagerInMulti>().camTransform = camera.transform;
            camera.GetComponent<CameraMove>().gameObject.SetActive(true);
            camera.GetComponent<CameraMove>()._player = _player;
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotaion);
        }
        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());

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
        GameObject _item = Instantiate(itemPrefabs[_itemType - (int)TileType.Item - 1], _position, Quaternion.identity);
        ItemManager _itemManager;
        _item.TryGetComponent<ItemManager>(out _itemManager);
        if(_itemManager != null)
        {
            _itemManager.Initialize(_itemId);
            items.Add(_itemId, _itemManager);
        }
    }

    public void SpawnEnemy(int _enemyId, Vector3 _position)
    {
        GameObject _enemy = Instantiate(enemyPrefab, _position, Quaternion.identity);
        _enemy.GetComponent<ServerEnemy>().Initialize(_enemyId);
        enemies.Add(_enemyId, _enemy.GetComponent<ServerEnemy>());
    }
}
