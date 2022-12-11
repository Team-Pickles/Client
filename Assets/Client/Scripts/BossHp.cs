using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHp : MonoBehaviour
{
    private int _hp = 1;
    public int Hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
}
