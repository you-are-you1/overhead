using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DottedTilemapScript : MonoBehaviour
{
    private int switchesCollected;
    private int switchesNeeded;

    private TileBase[] dottedTiles;
    private Tilemap solidTilemap;
    

    private BoundsInt combinedBounds;

    public TileBase solidRuleTile;
    public TileBase dottedRuleTile;

    public static event Action<DottedTilemapScript> OnSwitchTilemapEvent;

    private GameObject player;
    private bool isPlayerInDottedTiles;

    DottedSpikesScript ds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isPlayerInDottedTiles = false;
            
        switchesCollected = 0;
        switchesNeeded = GameObject.FindGameObjectsWithTag("Switch").Length;

        Tilemap dottedTilemap = GetComponent<Tilemap>();
        

        solidTilemap = GameObject.Find("Solid Tilemap").GetComponent<Tilemap>();
       
        combinedBounds = GetCombinedBounds(dottedTilemap, solidTilemap);
        dottedTiles = dottedTilemap.GetTilesBlock(combinedBounds);

        for (int i = 0; i < dottedTiles.Length; i++)
        {
            if (dottedTiles[i] == dottedRuleTile)
            {
                dottedTiles[i] = solidRuleTile;
            }
        }

        ds = GameObject.Find("Dotted Spikes").GetComponent<DottedSpikesScript>();

   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    private void SwitchToSolid(SwitchScript s)
    {
        switchesCollected++;
        if (switchesCollected == switchesNeeded)
        {
            

            StartCoroutine(CheckForPlayer());
        }
    }

    private IEnumerator CheckForPlayer()
    {


        while (isPlayerInDottedTiles || ds.isPlayerInDottedSpikes)
        {
            yield return null;
            
        }

        solidTilemap.SetTilesBlock(combinedBounds, dottedTiles);

        OnSwitchTilemapEvent?.Invoke(this);
    }

    public static BoundsInt GetCombinedBounds(Tilemap a, Tilemap b)
    {
        a.CompressBounds();
        b.CompressBounds();

        BoundsInt boundsA = a.cellBounds;
        BoundsInt boundsB = b.cellBounds;

        Vector3Int min = Vector3Int.Min(boundsA.min, boundsB.min);
        Vector3Int max = Vector3Int.Max(boundsA.max, boundsB.max);

        Vector3Int size = max - min;

        return new BoundsInt(min, size);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerInDottedTiles = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerInDottedTiles = false;
            
        }
    }

    private void OnEnable()
    {
        SwitchScript.OnSwitchCollectEvent += SwitchToSolid;
    }

    private void OnDisable()
    {
        SwitchScript.OnSwitchCollectEvent -= SwitchToSolid;
    }
}
