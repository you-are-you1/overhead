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
    }

    private void OnDisable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent -= reloadLevelSpikes;
        Ascend.NextLevelEvent -= loadNextLevel;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent -= reloadLevelDeathPlane;
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
        yield return new WaitForSeconds(0.01f); //wait for isDeath to get set

        Debug.Log(isDeath);
        if (isDeath) //death
        {
            

            yield return new WaitForSeconds(waitTimeForDeathTransition);

            deathTransition.SetActive(true);
            levelTransition.SetActive(false);

            
            deathTransitionAnim.SetTrigger("DeathTransition");

            yield return new WaitForSeconds(deathTransitionTime);

        }
        else
        {
            yield return new WaitForSeconds(waitTimeForLevelTransition);

            deathTransition.SetActive(false);
            levelTransition.SetActive(true);

            levelTransitionAnim.SetTrigger("LevelTransition");

            yield return new WaitForSeconds(levelTransitionTime);
        }

        SceneManager.LoadScene(level);
        
    }

    private void reloadLevelSpikes(SpikesScript script) { reloadLevel(); }
    private void reloadLevelDeathPlane(DeathPlaneScript script) { reloadLevel(); }
}
