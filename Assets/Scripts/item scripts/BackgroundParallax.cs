using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private float length, height;

    private GameObject cam;
    private Vector2 startPos;

    [SerializeField] private float parallaxEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        cam = Camera.main.gameObject;

       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 temp = cam.transform.position * (1 - parallaxEffect);   
        Vector2 distance = cam.transform.position * parallaxEffect;

        transform.position = startPos + distance;

        if (temp.x > startPos.x + length) startPos.x += length;
        else if (temp.x < startPos.x - length) startPos.x -= length;

        if (temp.y > startPos.y + height) startPos.y += height;
        else if (temp.y < startPos.y - height) startPos.y -= height;
    }
}
