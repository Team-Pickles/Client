using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollideAction : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        transform.parent.GetComponent<PlayerMove>().onGround = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        transform.parent.GetComponent<PlayerMove>().onGround = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
