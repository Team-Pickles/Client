using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorColiider : MonoBehaviour
{
    public DoorAction _door;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            _door.istouched = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            _door.istouched = false;
        }
    }
}
