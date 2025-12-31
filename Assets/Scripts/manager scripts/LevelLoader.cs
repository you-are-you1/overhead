using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float waitTimeForDeathTransition;
    [SerializeField] private float deathTransitionTime;
    [SerializeField] private float waitTimeForLevelTransition;
    [SerializeField] private float levelTransitionTime;

    public Animator deathTransitionAnim;
    public Animator levelTransitionAnim;

    private GameObject deathTransition, levelTransition;

    public static bool isDeath = false;
    
    private void OnEnable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent += reloadLevelSpikes;
        Ascend.NextLevelEvent += loadNextLevel;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent += reloadLevelDeathPlane;
        PauseScript.RetryLevelEvent += reloadLevelRetry;
        PauseScript.ReturnToMenuEvent += returnToStartPause;
    }

    private void OnDisable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent -= reloadLevelSpikes;
        Ascend.NextLevelEvent -= loadNextLevel;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent -= reloadLevelDeathPlane;
        PauseScript.RetryLevelEvent -= reloadLevelRetry;
        PauseScript.ReturnToMenuEvent -= returnToStartPause;
    }

    private void Awake()
    {
        deathTransition = transform.Find("DeathTransition").gameObject;
        levelTransition = transform.Find("LevelTransition").gameObject;
        

        deathTransition.SetActive(isDeath);
        levelTransition.SetActive(!isDeath);
    }

    private void reloadLevel()
    {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    private void loadNextLevel(Ascend script)
    {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator loadLevel(int level)
    {
        yield return new WaitForSecondsRealtime(0.01f); //wait for isDeath to get set

   
        if (isDeath) //death
        {
            

            
            yield return new WaitForSecondsRealtime(waitTimeForDeathTransition);

            deathTransition.SetActive(true);
            levelTransition.SetActive(false);

            
            deathTransitionAnim.SetTrigger("DeathTransition");

            yield return new WaitForSecondsRealtime(deathTransitionTime);

        }
        else
        {
            yield return new WaitForSecondsRealtime(waitTimeForLevelTransition);

            //Debug.Log("First wait");
            deathTransition.SetActive(false);
            levelTransition.SetActive(true);

            levelTransitionAnim.SetTrigger("LevelTransition");
            // Debug.Log("animation triger");

            yield return new WaitForSecondsRealtime(levelTransitionTime);
            //Debug.Log("second wait");
        }

        SceneManager.LoadScene(level);
        //Debug.Log("loaoded scene");
    }

    private void reloadLevelSpikes(SpikesScript script) { reloadLevel(); }
    private void reloadLevelDeathPlane(DeathPlaneScript script) { reloadLevel(); }

    private void reloadLevelRetry(PauseScript script) { reloadLevel(); }

    private void returnToStartPause(PauseScript script)
    {
        StartCoroutine(goToStart());
        //Debug.Log("return to menu");
    }

    private IEnumerator goToStart()
    {
        deathTransition.SetActive(false);
        levelTransition.SetActive(true);

        levelTransitionAnim.SetTrigger("LevelTransition");
        // Debug.Log("animation triger");

        yield return new WaitForSecondsRealtime(levelTransitionTime);
        //Debug.Log("second wait");

        GameObject pauseMenu = GameObject.FindGameObjectWithTag("PauseCanvas");
        pauseMenu.GetComponent<PauseScript>().UnPauseGame(false);

        SceneManager.LoadScene(0);
    }
}
