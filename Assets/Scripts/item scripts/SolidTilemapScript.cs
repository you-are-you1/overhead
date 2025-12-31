using UnityEngine;
using UnityEngine.Tilemaps;

public class SolidTilemapScript : MonoBehaviour
{
    private TileBase[] solidTiles;
    private Tilemap dottedTilemap;

    private BoundsInt combinedBounds;

    public TileBase solidRuleTile;
    public TileBase dottedRuleTile;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tilemap solidTilemap = GetComponent<Tilemap>();

        dottedTilemap = GameObject.Find("Dotted Tilemap").GetComponent<Tilemap>();

        combinedBounds = DottedTilemapScript.GetCombinedBounds(solidTilemap, dottedTilemap);

        solidTiles = solidTilemap.GetTilesBlock(combinedBounds);

        for (int i = 0; i < solidTiles.Length; i++)
        {
           
            if (solidTiles[i] == solidRuleTile)
            {
                solidTiles[i] = dottedRuleTile;
                
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchToDotted(DottedTilemapScript d)
    {
        dottedTilemap.SetTilesBlock(combinedBounds, solidTiles);
    
    }

    private void OnEnable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent += SwitchToDotted;
    }
    private void OnDisable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent -= SwitchToDotted;
    }
}
