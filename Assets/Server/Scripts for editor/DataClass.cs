
/*
    Made by limmon029 
 */
using System;
using UnityEngine;

public enum InfoTypes
{
    tile = 0, enemy, item, player
}


[Serializable]
public class DataClass
{
    [SerializeField] int infoType;
    [SerializeField] Vector3 pos;
    [SerializeField] int additionalInfo = -1;

    public DataClass(int infoType, Vector3 pos)
    {
        this.infoType = infoType;
        this.pos = pos;
    }

    public DataClass(int infoType, Vector3 pos, int additionalInfo)
    {
        this.infoType = infoType;
        this.pos = pos;
        this.additionalInfo = additionalInfo;
    }

    public Vector3 GetPos()
    {
        return pos;
    }


    public int GetAdditionalInfo()
    {
        return additionalInfo;
    }

    public override string ToString()
    {
        string datatype;
        string addInfo = "";
        switch ((InfoTypes)infoType)
        {
            case InfoTypes.tile:
                datatype = "Tile";
                addInfo = " - TileType: [" + additionalInfo + "]";
                break;
            case InfoTypes.enemy:
                datatype = "Enemy";
                addInfo = " - EnemyType: [" + additionalInfo + "]";
                break;
            case InfoTypes.item:
                datatype = "Item";
                addInfo = " - ItemType: [" + additionalInfo + "]";
                break;
            case InfoTypes.player:
                datatype = "Player";
                addInfo = " - PlayerType: [" + additionalInfo + "]";
                break;
            default:
                datatype = "Unkown";
                break;
        }

        return datatype + "(" + pos + ")" + addInfo;
    }
}
