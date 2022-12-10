using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : MonoBehaviour
{
    public int id;
    public bool isIndoor;
    public bool istouched;

    public void Initialize(int _id, bool _isIndoor)
    {
        id = _id;
        isIndoor = _isIndoor;
    }

    void Update()
    {
        if (istouched && isIndoor && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ClientSend.GoToNextPortal(id);
        }
    }
}
