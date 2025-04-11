using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimationScript : MonoBehaviour
{
    
    public Animator playerAnimator;
    private Rigidbody2D rb;
    

    private void Awake()
    {

       
        rb = GetComponent<Rigidbody2D>();  
    }
    private void OnEnable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent += killPlayerSpikes;
        Ascend.NextLevelEvent += transitionAnimation;
    }

    private void OnDisable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent -= killPlayerSpikes;
        Ascend.NextLevelEvent -= transitionAnimation;
    }

    private void killPlayerSpikes(SpikesScript script) { killPlayer(); }
    private void killPlayer()
    {
        
        rb.simulated = false;

        playerAnimator.SetTrigger("Death"); //player death anim
        LevelLoader.isDeath = true;
        Debug.Log(LevelLoader.isDeath);
    }

    private void transitionAnimation(Ascend script)
    {
        rb.simulated = false;

        playerAnimator.SetTrigger("LevelTransition");
        LevelLoader.isDeath = false;
    }
}
