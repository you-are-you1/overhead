using UnityEngine;

public class GrapplePointScript : MonoBehaviour
{

    private GameObject player;
    private GameObject indicator;

    private Vector2 directionFromPlayer;
    private float angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        indicator = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (indicator.activeSelf)
        {
            directionFromPlayer = indicator.transform.position - player.transform.position;
            angle = Mathf.Atan2(directionFromPlayer.y, directionFromPlayer.x) * Mathf.Rad2Deg;

            indicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player) indicator.SetActive(true);
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.gameObject == player) indicator.SetActive(false);
    }
}
