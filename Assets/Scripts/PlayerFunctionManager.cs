using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class PlayerFunctionManager
{
    private static PlayerFunctionManager instance = null;
    private PlayerFunctionManager()
    { 
    
    }

    public PlayerFunctionManager Instance
    {
        get
        {
            if (instance == null)
                instance = new PlayerFunctionManager();
            return instance;
        }
    }
    public void SetPlayerStateFlags(ref PlayerDataStorage ds, PlayerStateFlags flag)
    {
        _state |= flag;
    }
    public void ResetPlayerStateFlags(ref PlayerDataStorage ds, PlayerStateFlags flag)
    {
        _state &= ~flag;
    }
    public void IncreaseBullet()
    {
        _bulletCount++;
        //Debug.Log("DataStorage::BulletCount Increased");
    }
    public void DecreaseBullet()
    {
        _bulletCount--;
    }
    private IEnumerator Explosion(float hValue, float vValue)
    {
        SetPlayerStateFlags(PlayerStateFlags.Stun);
        _hPoint = hValue;
        _vPoint = vValue;
        yield return new WaitForSeconds(2);
        ResetPlayerStateFlags(PlayerStateFlags.Stun);
        yield break;
    }
}
*/