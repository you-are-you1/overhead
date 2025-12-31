using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using TMPro;

public class EndMenuScript : MonoBehaviour
{

    public float sceneLoadWaitTime;
    public float fadeInWaitTime;

    public Vector2 backgroundScroll;

    public TMP_Text timeText, deathText, coinText;

    private InputSystemUIInputModule UIInput;

    private Animator animator;

    private TimerScript t;

    public static event Action<EndMenuScript> RestartButtonEvent;
    public static event Action<EndMenuScript> EndScreenEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EndScreenEvent?.Invoke(this);

        animator = GetComponent<Animator>();
        UIInput = GameObject.FindGameObjectWithTag("Event System")
            .GetComponent<InputSystemUIInputModule>();

        t = GameObject.FindGameObjectWithTag("SpeedrunTimer").GetComponent<TimerScript>();

        StartCoroutine(waitForStart());

        StartBackgroundScroll(backgroundScroll);

        timeText.SetText(TimeSpan.FromSeconds(t.getTime()).ToString("mm':'ss'.'fff"));
        deathText.SetText(AnimationScript.deaths.ToString());
        coinText.SetText(CoinCollectScript.totalCoins.ToString() + "/10");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator waitForStart()
    {
        yield return new WaitForSeconds(fadeInWaitTime);
        UIInput.enabled = true;
    }

    public void RestartGame()
    {
        StartCoroutine(loadStart());
    }

    private IEnumerator loadStart()
    {
        UIInput.enabled = false;

        animator.SetTrigger("Start");

        RestartButtonEvent?.Invoke(this);
        CoinCollectScript.totalCoins = 0;
        AnimationScript.deaths = 0;
        yield return new WaitForSeconds(sceneLoadWaitTime);



        SceneManager.LoadScene(0);
    }

    private void StartBackgroundScroll(Vector2 scroll)
    {
        Camera.main.gameObject.transform.Find("background grid").gameObject.
            GetComponent<SpriteRenderer>().material.SetVector("_scrollDirection", scroll);
    }
}
