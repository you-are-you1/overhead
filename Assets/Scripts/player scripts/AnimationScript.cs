using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimationScript : MonoBehaviour
{
    
    public Animator playerAnimator;
    private Rigidbody2D rb;

    private Ascend ascend;
    private PlayerMovementWithDash pm;

    public static int deaths = 0;
    private void Awake()
    {

        ascend = GetComponent<Ascend>();
        pm = GetComponent<PlayerMovementWithDash>();
        rb = GetComponent<Rigidbody2D>();  
    }
    private void OnEnable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent += killPlayerSpikes;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent += killPlayerDeathPlane;
        PauseScript.RetryLevelEvent += killPlayerRetry;
        Ascend.NextLevelEvent += transitionAnimation;
    }

    private void OnDisable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent -= killPlayerSpikes;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent -= killPlayerDeathPlane;
        PauseScript.RetryLevelEvent -= killPlayerRetry;
        Ascend.NextLevelEvent -= transitionAnimation;
    }

    private void killPlayerSpikes(SpikesScript script) { killPlayer(); }
    private void killPlayerDeathPlane(DeathPlaneScript script) { killPlayer(); }

    private void killPlayerRetry(PauseScript script) { killPlayer(); }
    private void killPlayer()
    {
        deaths++;
      

        rb.simulated = false;
        ascend.abilityAction.Disable();
        pm.moveAction.Disable();
        pm.jumpAction.Disable();
        ascend.checkForAscend = false;
        PauseScript.canPause = false;
        playerAnimator.SetTrigger("Death"); //player death anim
        AudioManager.instance.Play("Death");
        //LevelLoader.isDeath = true;
       
    }

    private void transitionAnimation(Ascend script)
    {
        rb.simulated = false;

        playerAnimator.SetTrigger("LevelTransition");
        LevelLoader.isDeath = false;
    }
}
