using System.Collections;
using UnityEngine;

public class Vacuum : Skill
{
    GameObject _player;
    GameObject _oldEnemy, _curEnemy;

    public override void OnChange()
    {
        _player = GameObject.Find("Player");
        Debug.Log("Vacuum::Start");
    }
    public override void OnStart()
    {
        _player = GameObject.Find("Player");
        Debug.Log("Vacuum::Start");
    }
    public override void OnSkill()
    {
        Vector2 rayOrigin = new Vector2(_player.transform.position.x + _player.transform.localScale.x / 2.0f * 1.01f, _player.transform.position.y - _player.transform.localScale.y/4);
        float length = 8.0f;

        _curEnemy = null;
        for (int i = -5; i <= 5; i++)
        {
            Vector2 rd = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rd, length);

            if (hit.collider != null)
            {
                Debug.DrawLine(rayOrigin, hit.point, Color.green);
                if (hit.transform.CompareTag("enemy")) // �ٲ� ���� ����
                {
                    _curEnemy = hit.transform.gameObject;
                    _curEnemy.GetComponent<Rigidbody2D>().AddForce((hit.point - rayOrigin).normalized * -0.3f);
                    if (_oldEnemy == null)
                        _curEnemy.GetComponent<Enemy>().OnCaptive();
                }
            }
        }
        if (_oldEnemy != null && _curEnemy == null)
        {
            _oldEnemy.GetComponent<Enemy>().OnReleased();
        }
        _oldEnemy = _curEnemy;
    }
    public override IEnumerator OnFire()
    {
        //PlayerDataStorage ds = PlayerDataStorage.Instance;
        PlayerMoveManager pmm = _player.GetComponent<PlayerMoveManager>();
        bool flip = _player.GetComponent<SpriteRenderer>().flipX;
        if (pmm.BulletCount > 0)
        {
            pmm.DecreaseBullet();

            GameObject bullet;
            if(!flip)
            {
                bullet = Object.Instantiate(pmm.bulletPrefab, new Vector3(_player.transform.position.x + _player.transform.localScale.x / 2.0f * 1.1f, _player.transform.position.y, 0), new Quaternion());
                bullet.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(500.0f, 0.0f));
                bullet.transform.GetComponent<Rigidbody2D>().angularVelocity = 500.0f;
                yield return new WaitForSeconds(2.0f);
                Object.Destroy(bullet);
            }
            else
            {
                bullet = Object.Instantiate(pmm.bulletPrefab, new Vector3(_player.transform.position.x - _player.transform.localScale.x / 2.0f * 1.1f, _player.transform.position.y, 0), new Quaternion());
                bullet.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500.0f, 0.0f));
                bullet.transform.GetComponent<Rigidbody2D>().angularVelocity = 500.0f;
                yield return new WaitForSeconds(2.0f);
                Object.Destroy(bullet);
            }
        }
        yield return 0;
    }
    public override void OnEnd()
    {
        if (_oldEnemy != null)
            _oldEnemy.GetComponent<Enemy>().OnReleased();

        _oldEnemy = null;
        _curEnemy = null;
        Debug.Log("Vacuum::End");
    }
}