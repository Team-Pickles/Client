using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool onGround = false;
    private float xAxisDrag = 0.005f;
    private float hPoint = 0, vPoint = 0;
    private float hSpeed = 4.0f, vSpeed = 5.0f;
    private bool leftPressed = false, rightPressed = false;

    enum State
    {
        Normal, Stun
    }
    State state = State.Normal;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "tnt":
            {
                OnExplosionAction();
                break;
            }
        }
    }
    public void OnJumpAction()
    {
        vPoint = 1.0f;
    }
    private IEnumerator Explosion()
    {
        state = State.Stun;
        hPoint = 5.0f;
        yield return new WaitForSeconds(2);
        state = State.Normal;
        yield break;
    }
    public void OnExplosionAction()
    {
        StartCoroutine(Explosion());
    }
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(hPoint * hSpeed, GetComponent<Rigidbody2D>().velocity.y + vPoint * vSpeed);
        vPoint = 0.0f;
    }
    void Update()
    {
        // ю╖
        

        // аб©Л
        leftPressed = Input.GetKey(KeyCode.LeftArrow);
        rightPressed = Input.GetKey(KeyCode.RightArrow);

        switch (state)
        {
            case State.Normal:
            {
                //Debug.Log("Normal");
                if (onGround && Input.GetKeyDown(KeyCode.UpArrow))
                    OnJumpAction();
                hPoint = (leftPressed == true ? -1 : 0) + (rightPressed == true ? 1 : 0);
                break;
            }
            case State.Stun:
            {
                //Debug.Log("Stun..");
                //hPoint = hPoint / (xAxisDrag + 1.0f);
                break;
            }
        }
        hPoint = hPoint / (xAxisDrag + 1.0f);
    }
}
