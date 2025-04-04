using System;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.InputSystem;


public class DeathScript : MonoBehaviour
{
    
    public Animator deathAnimation;
    private Rigidbody2D rb;
    

    private void Awake()
    {
       

        rb = GetComponent<Rigidbody2D>();  
    }
    private void OnEnable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent += killPlayerSpikes;
        
    }

    private void OnDisable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent -= killPlayerSpikes;
        
    }

    private void killPlayerSpikes(SpikesScript script) { killPlayer(); }
    private void killPlayer()
    {
        
        rb.simulated = false;

        deathAnimation.SetTrigger("Death");
    }
}
