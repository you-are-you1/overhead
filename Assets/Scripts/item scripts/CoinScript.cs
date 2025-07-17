using System;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public static event Action<CoinScript> OnCollectCoinEvent;

    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            OnCollectCoinEvent?.Invoke(this);
            gameObject.SetActive(false);
        } 
    }
}
