using System;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    private GameObject player;
    private Animator switchAnimator;
    private bool collected;

    public static event Action<SwitchScript> OnSwitchCollectEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        switchAnimator = GetComponent<Animator>();
        collected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && !collected)
        {
            collected = true;
            switchAnimator.SetTrigger("Collected");
            OnSwitchCollectEvent?.Invoke(this);
        }
    }
}
