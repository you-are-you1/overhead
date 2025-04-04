using System;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{

    public static event Action<SpikesScript> OnPlayerTouchSpikesEvent;

    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            Debug.Log("player touched spikes");
            OnPlayerTouchSpikesEvent?.Invoke(this); //send spikes event out

        }
    }
}
