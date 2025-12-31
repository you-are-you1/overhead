using UnityEngine;

public class GridScrollDirection : MonoBehaviour
{
    public float scrollSpeed;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject ending = GameObject.FindGameObjectWithTag("Level Trigger");

        Vector2 scrollDir = (ending.transform.position - player.transform.position).normalized * -scrollSpeed;

        GetComponent<SpriteRenderer>().material.SetVector("_scrollDirection", scrollDir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
