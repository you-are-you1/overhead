using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DottedTilemapScript : MonoBehaviour
{
    private int switchesCollected;
    private int switchesNeeded;

    private TileBase[] dottedTiles;
    private Tilemap solidTilemap;

    private Tilemap dottedTilemap;

    private BoundsInt combinedBounds;

    public TileBase solidRuleTile;
    public TileBase dottedRuleTile;

    public static event Action<DottedTilemapScript> OnSwitchTilemapEvent;

    private GameObject player;
    private bool isPlayerInDottedTiles;

    DottedSpikesScript ds;

    private List<Vector3Int> upEdges;
    private List<Vector3Int> downEdges;
    private List<Vector3Int> leftEdges;
    private List<Vector3Int> rightEdges;

    public GameObject TilemapSwitchPS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isPlayerInDottedTiles = false;
            
        switchesCollected = 0;
        switchesNeeded = GameObject.FindGameObjectsWithTag("Switch").Length;

        dottedTilemap = GetComponent<Tilemap>();
        

        solidTilemap = GameObject.Find("Solid Tilemap").GetComponent<Tilemap>();
       
        combinedBounds = GetCombinedBounds(dottedTilemap, solidTilemap);
        dottedTiles = dottedTilemap.GetTilesBlock(combinedBounds);

        upEdges = new List<Vector3Int>();
        downEdges = new List<Vector3Int>();
        leftEdges = new List<Vector3Int>();
        rightEdges = new List<Vector3Int>();

        

        for (int i = 0; i < dottedTiles.Length; i++)
        {
            if (dottedTiles[i] == dottedRuleTile)
            {
                dottedTiles[i] = solidRuleTile;
            }
        }

        for (int x = combinedBounds.x; x < combinedBounds.x + combinedBounds.size.x; x++)
        {
            for (int y = combinedBounds.y; y < combinedBounds.y + combinedBounds.size.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (dottedTilemap.GetTile(pos) == dottedRuleTile)
                {
                    if (dottedTilemap.GetTile(pos + Vector3Int.up) == null) upEdges.Add(pos);

                    if (dottedTilemap.GetTile(pos + Vector3Int.down) == null) downEdges.Add(pos);   

                    if (dottedTilemap.GetTile(pos + Vector3Int.left) == null) leftEdges.Add(pos);

                    if (dottedTilemap.GetTile(pos + Vector3Int.right) == null) rightEdges.Add(pos);
                }
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

        AudioManager.instance.Play("TilemapSwitch");

        spawnParticles();
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

    private void spawnParticles()
    {
        Vector3Int offset = new Vector3Int(1, 1, 0);
        foreach (Vector3Int pos in upEdges)
        {
            Instantiate(TilemapSwitchPS, pos + offset, Quaternion.identity);
        }

        foreach (Vector3Int pos in downEdges)
        {
            Instantiate(TilemapSwitchPS, pos + offset, Quaternion.Euler(0f, 0f, 180f));
        }

        foreach (Vector3Int pos in leftEdges)
        {
            Instantiate(TilemapSwitchPS, pos + offset, Quaternion.Euler(0f, 0f, 90f));
        }

        foreach (Vector3Int pos in rightEdges)
        {
            Instantiate(TilemapSwitchPS, pos + offset, Quaternion.Euler(0f, 0f, 270f));
        }
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
