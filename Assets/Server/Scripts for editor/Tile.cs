using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum TileType
{
    Empty = 0,
    tiles_packed_40, tiles_packed_41, tiles_packed_42, tiles_packed_43, PlatformerTiles_2,
    tiles_packed_60, tiles_packed_61, tiles_packed_62, tiles_packed_63, PlatformerTiles_0,
    tiles_packed_120, tiles_packed_121, tiles_packed_122, tiles_packed_123, PlatformerTiles_1,
    tiles_packed_140, tiles_packed_141, tiles_packed_142, tiles_packed_143,
    tiles_packed_36,  tiles_packed_17, tiles_packed_18, tiles_packed_19,
    tiles_packed_56, tiles_packed_37, tiles_packed_38, tiles_packed_39, tiles_packed_68,
    tiles_packed_76, tiles_packed_57, tiles_packed_58, tiles_packed_59, tiles_packed_88,
    tiles_packed_80, tiles_packed_81, tiles_packed_82, tiles_packed_83, tiles_packed_87,
    tiles_packed_100, tiles_packed_101, tiles_packed_102, tiles_packed_103,
    
    Item =100,
    trash,  tiles_packed_151, tiles_packed_44,  Spring_Sheet_0, grenade2,
    rope_long, rope_short, barricade,
    Enemy =200,
    Boss, can_stand_Sheet_2,
    
    Player = 300,
    Charactor_Sheet_0,
    
    BackGround = 400,
    ArcadeGreyBackground,
    BlueBackground,
    BrickBackground,
    DarkBackground,
    DustyBackground,
    FieldBackground,
    GreenBackground,
    SkyBackground,

    MapSize = 500,
    minSize, maxSize,

    door=600,
    indoor, outdoor
}

/*
    tiles_packed_0, tiles_packed_1, tiles_packed_2, tiles_packed_3, tiles_packed_4,
    tiles_packed_5, tiles_packed_6, tiles_packed_12, tiles_packed_13, tiles_packed_14,
    tiles_packed_15, tiles_packed_16, tiles_packed_17, tiles_packed_18, tiles_packed_19,
    tiles_packed_20, tiles_packed_21, tiles_packed_22, tiles_packed_23, tiles_packed_24,
    tiles_packed_25, tiles_packed_26, tiles_packed_33, tiles_packed_34, tiles_packed_35,
    tiles_packed_36, tiles_packed_37, tiles_packed_38, tiles_packed_39, tiles_packed_40,
    tiles_packed_41, tiles_packed_42, tiles_packed_43, tiles_packed_53, tiles_packed_54,
    tiles_packed_55, tiles_packed_56, tiles_packed_57, tiles_packed_58, tiles_packed_59,
    tiles_packed_60, tiles_packed_61, tiles_packed_62, tiles_packed_63, tiles_packed_68,
    tiles_packed_80, tiles_packed_81, tiles_packed_82, tiles_packed_83, tiles_packed_100,
    tiles_packed_101, tiles_packed_102, tiles_packed_103, tiles_packed_120, tiles_packed_121,
    tiles_packed_122, tiles_packed_123, tiles_packed_140, tiles_packed_141, tiles_packed_142,
    tiles_packed_143, PlatformerTiles_0, PlatformerTiles_1, PlatformerTiles_2,
    */

public class Tile : MonoBehaviour
{
    [SerializeField]
    public Sprite[] tileImage;
    [SerializeField]
    private Sprite[] itemImage;
    [SerializeField]
    private Sprite[] enemyImage;
    [SerializeField]
    public Sprite playerImage;

    private TileType tileType;
    private SpriteRenderer spriteRenderer;

    public void Setup(TileType _tileType)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        tileType = _tileType;
    }

    public TileType TileType
    {
        set
        {
            tileType = value;
        }

        get => tileType;
    }

}
