using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DottedSpikesScript : MonoBehaviour
{
    [HideInInspector] public bool isPlayerInDottedSpikes;
    private GameObject player;

    private TileBase[] dottedSpikes;
    private Tilemap solidSpikesTilemap;

    private BoundsInt combinedBounds;

    public TileBase[] dottedSpikesArray;
    public TileBase[] solidSpikesArray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        Tilemap dottedSpikesTilemap = GetComponent<Tilemap>();

        solidSpikesTilemap = GameObject.Find("Solid Spikes").GetComponent<Tilemap>();

        combinedBounds = DottedTilemapScript.GetCombinedBounds(dottedSpikesTilemap, solidSpikesTilemap);

        dottedSpikes = dottedSpikesTilemap.GetTilesBlock(combinedBounds);

        for (int i = 0; i < dottedSpikes.Length; i++)
        {
            int index = Array.IndexOf(dottedSpikesArray, dottedSpikes[i]);
            if (index != -1)
            {
                dottedSpikes[i] = solidSpikesArray[index];
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchToSolid(DottedTilemapScript d)
    {
        solidSpikesTilemap.SetTilesBlock(combinedBounds, dottedSpikes);
    }

    private void OnEnable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent += SwitchToSolid;
    }
    private void OnDisable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent -= SwitchToSolid;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerInDottedSpikes = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerInDottedSpikes = false; 
        }
    }
}
