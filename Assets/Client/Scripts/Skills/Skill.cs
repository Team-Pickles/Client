using System.Collections;
using UnityEngine;

public class Skill
{
    protected GameObject _player = GameObject.Find("Player");
    protected GameObject _firePoint = GameObject.Find("FirePoint");
    public virtual void OnChange() { } // change animation
    public virtual void OnStart() { } // start frame
    public virtual void OnConsume() // attack
    {
        bool flip = _player.transform.localScale.x > 0 ? false : true;
        Vector2 rayOrigin = _firePoint.transform.position;

        float length = 8.0f;

        for (int i = -5; i <= 5; i++)
        {
            Vector2 rayDirection = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad) * (flip == false ? 1 : -1), Mathf.Sin(i * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, length);

            if (hit.collider != null)
            {
                Debug.DrawLine(rayOrigin, hit.point, Color.green);
                if (hit.transform.CompareTag("trash"))
                {
                    hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce((hit.point - rayOrigin).normalized * -3.0f);
                    hit.transform.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 200.0f;
                }
            }
        }
    }
    public virtual IEnumerator OnFire()
    {
        PlayerMoveManager pmm = _player.GetComponent<PlayerMoveManager>();

        if (pmm.BulletCount > 0)
        {
            pmm.DecreaseBullet();

            GameObject bullet;
            int isFliped = _player.transform.localScale.x > 0 ? 1 : -1;
            bullet = Object.Instantiate(pmm.bulletPrefab, _firePoint.transform.position, new Quaternion());
            bullet.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(500.0f * isFliped, 0.0f));
            bullet.transform.GetComponent<Rigidbody2D>().angularVelocity = 500.0f;

            Vector3 scale = bullet.transform.GetComponent<Rigidbody2D>().transform.localScale;
            bullet.transform.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(isFliped * scale.x, isFliped * scale.y, isFliped * scale.z);
            yield return new WaitForSeconds(2.0f);
            Object.Destroy(bullet);
        }
    }
    public virtual IEnumerator OnItemUse() { yield break; }
    public virtual void OnEnd() { }   // return animation
}
