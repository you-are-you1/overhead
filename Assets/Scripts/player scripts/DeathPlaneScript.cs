using System;
using UnityEngine;

public class DeathPlaneScript : MonoBehaviour
{
    [SerializeField] private GameObject deathPlane;

    public static event Action<DeathPlaneScript> OnPlayerTouchDeathPlaneEvent;
    private bool hasTouchedDeathPlane;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasTouchedDeathPlane = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < deathPlane.transform.position.y && !hasTouchedDeathPlane)
        {
            LevelLoader.isDeath = true;
            hasTouchedDeathPlane = true;
            OnPlayerTouchDeathPlaneEvent?.Invoke(this);
        }
    }
}
