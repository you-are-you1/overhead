using System;
using UnityEngine;

public class AscendBlockerScript : MonoBehaviour
{
    public static event Action<AscendBlockerScript> OnPlayerTouchAscendBlocker;

    public GameObject player;

    private Ascend playerAscend;
    private LineDrawScript lds;

    public GameObject ascendBlockEffect;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAscend = player.GetComponent<Ascend>();
        lds = player.GetComponent<LineDrawScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAscend.isAscending && gameObject.layer == 6)
        {
            gameObject.layer = 8;
        }
        if (gameObject.layer == 8 && playerAscend.isAscendingInWall())
        {
            gameObject.layer = 6;
        }
        if (gameObject.layer == 8 && !playerAscend.isAscending)
        {
            gameObject.layer = 6;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject == player)
        {
           
            if (playerAscend.isAscending)
            {
                OnPlayerTouchAscendBlocker?.Invoke(this);
                lds.disableTrails();
                Instantiate(ascendBlockEffect, player.transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            }
        }
    }
}
