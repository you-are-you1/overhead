using System;
using Unity.Burst;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{

    public static event Action<SpikesScript> OnPlayerTouchSpikesEvent;

    public GameObject player;
    private Ascend playerAscend;
    private PlatformEffector2D platformEffector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAscend = player.GetComponent<Ascend>();
        platformEffector = GetComponent<PlatformEffector2D>();
        platformEffector.useOneWay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAscend.isAscendingInWall() && !platformEffector.useOneWay)
        {
            platformEffector.useOneWay = true;
        }
        else if (!playerAscend.isAscendingInWall() && platformEffector.useOneWay)
        {
            platformEffector.useOneWay = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject == player && !playerAscend.isAscendingInWall())
        {
            Debug.Log("playuer touched spikes");
            OnPlayerTouchSpikesEvent?.Invoke(this); //send spikes event out

        }
    }
}
