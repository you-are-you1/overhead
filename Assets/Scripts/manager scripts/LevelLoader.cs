using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float waitTimeForTransition;
    [SerializeField] private float transitionTime;

    public Animator deathTransition;
    private void OnEnable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent += reloadLevelSpikes;

    }

    private void OnDisable()
    {
        SpikesScript.OnPlayerTouchSpikesEvent -= reloadLevelSpikes;

    }

    private void reloadLevel()
    {
        StartCoroutine(loadLevelAfterDeath());
    }

    IEnumerator loadLevelAfterDeath()
    {
        yield return new WaitForSeconds(waitTimeForTransition);

        deathTransition.SetTrigger("DeathTransition");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void reloadLevelSpikes(SpikesScript script) { reloadLevel(); }
}
