using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TimerScript : MonoBehaviour
{

    public TMP_Text timeText;

    private TimeSpan timePlaying;
    private bool isTimerRunning;

    private float elapsedTime = 0f;

    private GameObject timerObject;

    public float transitionTime;

    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("SpeedrunTimer");

        if (objects.Length > 1) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeText.text = "00:00.00";
        isTimerRunning = false;

        timerObject = transform.Find("timer background").gameObject;
        
        if (!StartMenuScript.isTimerOn) timerObject.SetActive(false);
        else timerObject.SetActive(true);

    }

    private void PauseTimer(Ascend a)
    {
        isTimerRunning = false;
    }

    private void ResumeTimer(Ascend a)
    {
        if (!isTimerRunning)
        {
            isTimerRunning = true;
            StartCoroutine(UpdateTimer());
        }
    }

    private void ResetTimer(EndMenuScript e)
    {
        elapsedTime = 0f;
    }

    private void ResetTimerPause(PauseScript p)
    {
        StartCoroutine(HideAndResetTimer());
    }

    private IEnumerator HideAndResetTimer()
    {
        yield return new WaitForSecondsRealtime(transitionTime);
        isTimerRunning = false;
        elapsedTime = 0f;
        timeText.text = "00:00.00";
        timerObject.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1 && StartMenuScript.isTimerOn && timerObject != null) timerObject.SetActive(true);
    }
    
    private void HideTimer(EndMenuScript e)
    {
        timerObject.SetActive(false);
    }

    private void ShowTimer(bool isTimer)
    {
        timeText.text = "00:00.00";
        timerObject.SetActive(isTimer);
    }

    private void OnEnable()
    {
        Ascend.NextLevelEvent += PauseTimer;
        Ascend.StartGameplayEvent += ResumeTimer;
        EndMenuScript.RestartButtonEvent += ResetTimer;
        EndMenuScript.EndScreenEvent += HideTimer;
        StartMenuScript.StartButtonEvent += ShowTimer;
        PauseScript.ReturnToMenuEvent += ResetTimerPause;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Ascend.NextLevelEvent -= PauseTimer;
        Ascend.StartGameplayEvent -= ResumeTimer;
        EndMenuScript.RestartButtonEvent -= ResetTimer;
        EndMenuScript.EndScreenEvent -= HideTimer;
        StartMenuScript.StartButtonEvent -= ShowTimer;
        PauseScript.ReturnToMenuEvent -= ResetTimerPause;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private IEnumerator UpdateTimer()
    {
        while (isTimerRunning)
        {
            elapsedTime += Time.unscaledDeltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);

            timeText.text = timePlaying.ToString("mm':'ss'.'ff");

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getTime()
    {
        return elapsedTime;
    }
}
