using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    public float sceneLoadWaitTime;
    public float fadeInWaitTime;

    public Vector2 backgroundScroll;

    private InputSystemUIInputModule UIInput;

    private Animator animator;

    public static bool isTimerOn = false;
    public static bool isMute = false;

    private GameObject timerToggleInside, muteToggleInside;

    public static event Action<bool> StartButtonEvent;

    public static event Action<StartMenuScript> MuteToggleEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        //Screen.SetResolution(960, 540, false);
        //Application.targetFrameRate = 60;
        //Time.timeScale = 1.0f;
        //PauseScript.isPaused = false;
        //PauseScript.ignoreInput = false;
    }
    void Start()
    {
     
       
        animator = GetComponent<Animator>();
       
        UIInput =  GameObject.FindGameObjectWithTag("Event System")
            .GetComponent<InputSystemUIInputModule>();

        StartCoroutine(waitForStart(true));

        timerToggleInside = transform.Find("toggle inside").gameObject;
        timerToggleInside.SetActive(isTimerOn);

        muteToggleInside = transform.Find("mute toggle inside").gameObject;
        muteToggleInside.SetActive(isMute);

        StartBackgroundScroll(backgroundScroll);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        AudioManager.instance.Play("UISelect");
        StartCoroutine(loadNextScene());
    }

    public void QuitGame()
    {
        AudioManager.instance.Play("UISelect");
        UIInput.enabled = false;
        animator.SetTrigger("Start");
        StartCoroutine(FadeOutAndQuit());


  
    }

    private IEnumerator FadeOutAndQuit()
    {
        yield return new WaitForSecondsRealtime(fadeInWaitTime);
        Application.Quit();
    }

    public void ToggleTimer()
    {
        AudioManager.instance.Play("UISelect");
        isTimerOn = !isTimerOn;
        timerToggleInside.SetActive(isTimerOn);
       
    }

    public void ToggleMute()
    {
        isMute = !isMute;
        muteToggleInside.SetActive(isMute);
        MuteToggleEvent?.Invoke(this);
        AudioManager.instance.Play("UISelect");
    }

    public void PlayButtonHoverSound()
    {
        AudioManager.instance.Play("UIMove");
    }

    private IEnumerator loadNextScene()
    {
        UIInput.enabled = false;

        animator.SetTrigger("Start");
        yield return new WaitForSeconds(sceneLoadWaitTime);

        StartButtonEvent?.Invoke(isTimerOn);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator waitForStart(bool enableInput)
    {
        yield return new WaitForSeconds(fadeInWaitTime);
        if (enableInput) UIInput.enabled = true;
    }

    private void StartBackgroundScroll(Vector2 scroll)
    {
        Camera.main.gameObject.transform.Find("background grid").gameObject.
            GetComponent<SpriteRenderer>().material.SetVector("_scrollDirection", scroll);
    }
}
