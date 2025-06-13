using System;
using UnityEngine;

public class DeathPlaneScript : MonoBehaviour
{
    [SerializeField] private GameObject deathPlane;

    public static event Action<DeathPlaneScript> OnPlayerTouchDeathPlaneEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < deathPlane.transform.position.y)
        {
            LevelLoader.isDeath = true;
            OnPlayerTouchDeathPlaneEvent?.Invoke(this);
        }
    }
}
