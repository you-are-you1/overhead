using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class dottedSpikesScript : MonoBehaviour
{
    private TileBase[] solidSpikes;
    private Tilemap dottedSpikesTilemap;

    private BoundsInt combinedBounds;

    public TileBase[] solidSpikesArray;
    public TileBase[] dottedSpikesArray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tilemap solidSpikesTilemap = GetComponent<Tilemap>();

        dottedSpikesTilemap = GameObject.Find("Dotted Spikes").GetComponent<Tilemap>();

        combinedBounds = DottedTilemapScript.GetCombinedBounds(solidSpikesTilemap, dottedSpikesTilemap);

        solidSpikes = solidSpikesTilemap.GetTilesBlock(combinedBounds);

        for (int i = 0; i < solidSpikes.Length; i++)
        {
            int index = Array.IndexOf(solidSpikesArray, solidSpikes[i]);
            if (index != -1)
            {
                solidSpikes[i] = dottedSpikesArray[index];
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void SwitchToDotted(DottedTilemapScript d)
    {
        dottedSpikesTilemap.SetTilesBlock(combinedBounds, solidSpikes);
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
