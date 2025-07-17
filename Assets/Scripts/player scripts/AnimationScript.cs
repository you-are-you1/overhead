using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimationScript : MonoBehaviour
{
    
    public Animator playerAnimator;
    private Rigidbody2D rb;

    private Ascend ascend;
    

    private void Awake()
    {

        ascend = GetComponent<Ascend>();
        rb = GetComponent<Rigidbody2D>();  
    }
    private void OnEnable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent += killPlayerSpikes;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent += killPlayerDeathPlane;
        Ascend.NextLevelEvent += transitionAnimation;
    }

    private void OnDisable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent -= killPlayerSpikes;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent -= killPlayerDeathPlane;
        Ascend.NextLevelEvent -= transitionAnimation;
    }

    private void killPlayerSpikes(SpikesScript script) { killPlayer(); }
    private void killPlayerDeathPlane(DeathPlaneScript script) { killPlayer(); }
    private void killPlayer()
    {
        
        rb.simulated = false;
        ascend.abilityAction.Disable();
        ascend.checkForAscend = false;
        playerAnimator.SetTrigger("Death"); //player death anim
        //LevelLoader.isDeath = true;
        Debug.Log(LevelLoader.isDeath);
    }

    private void transitionAnimation(Ascend script)
    {
        rb.simulated = false;

        playerAnimator.SetTrigger("LevelTransition");
        LevelLoader.isDeath = false;
    }
}
