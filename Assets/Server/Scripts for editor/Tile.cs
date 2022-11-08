using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum TileType
{
    Empty = 0,
    tiles_packed_36, tiles_packed_37, tiles_packed_38, tiles_packed_39,
    tiles_packed_40, tiles_packed_41, tiles_packed_42, tiles_packed_43,
    tiles_packed_56, tiles_packed_57, tiles_packed_58, tiles_packed_59,
    tiles_packed_60, tiles_packed_61, tiles_packed_62, tiles_packed_63,
    tiles_packed_80, tiles_packed_81, tiles_packed_82, tiles_packed_83,
    tiles_packed_87, tiles_packed_88, tiles_packed_68, tiles_packed_112,tiles_packed_108,
    PlatformerTiles_2, PlatformerTiles_0, PlatformerTiles_1,
   /*
    stickyTopAlone, stickyMidLeft, stickyMidCentor, stickyMidRight,
    sandAlone, sandMidLeft, sandMidCentor, sandMidRight,
    stickyMidAlone, stickyBottomLeft, stickyBottomCentor, stickyBottomRight,
    sandTopAlone, sandTopeLeft, sandTopCentor, sandTopRight,
    IcedGroundAlone, IcedGroundLeft, IcedMIdAlone, IcedGroundRight,
    leftArrow, rightArrow, spike, flag, jumpUp,
    glass, rock, bricks,
   */
    Item =100,
    tiles_packed_151, tiles_packed_44, Bullet,
    Enemy=200,
    Sprite_Boss,
    Player = 300,
    Charactor_Sheet_0,
}

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
