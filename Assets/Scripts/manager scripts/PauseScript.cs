using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PauseScript : MonoBehaviour
{
    public InputSystem_Actions controls;
    public InputAction pauseAction;

    private GameObject EventSystem;
    private Button ContinueButton, RetryButton, ExitButton;
   
    private TMP_Text RetryButtonText;


    private InputSystemUIInputModule EventSystemInput;

    public static bool isPaused, ignoreInput;
    public static bool canPause, canUnpause;

    private GameObject pauseMenu;

    public static event Action<PauseScript> RetryLevelEvent;
    public static event Action<PauseScript> ReturnToMenuEvent;

    public Color retryDisabledColor, defaultColor;

    private AudioManager audioManager;
    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("SpeedrunTimer");

        if (objects.Length > 1) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);



        pauseAction = InputSystem.actions.FindAction("Pause");

        isPaused = false;
        canPause = false;
        canUnpause = true;

        pauseMenu = transform.Find("pauseMenu").gameObject;
        Transform panel = pauseMenu.transform.Find("Panel");

        ContinueButton = panel.Find("continue button").GetComponent<Button>();
        RetryButton = panel.Find("retry button").GetComponent<Button>();
        ExitButton = panel.Find("return to title button").GetComponent<Button>();
        RetryButtonText = RetryButton.transform.GetChild(0).GetComponent<TMP_Text>();

        defaultColor = RetryButtonText.color;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.WasPressedThisFrame() && canPause)
        {
            if (!isPaused) PauseGame();
            else if (canUnpause) UnPauseGame(true);

        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        ignoreInput = true;
        pauseMenu.SetActive(isPaused);
        Ascend playerAscend = GameObject.FindGameObjectWithTag("Player").GetComponent<Ascend>();
        playerAscend.checkForAscend = false;

        audioManager.Play("UISelect");

        EventSystem = GameObject.FindGameObjectWithTag("Event System");
        EventSystemInput = EventSystem.GetComponent<InputSystemUIInputModule>();
        EventSystemInput.enabled = true;
        EventSystem.GetComponent<EventSystem>().SetSelectedGameObject(ContinueButton.gameObject);

        ChangeRetryButton(playerAscend.isAscending);
        ExitButton.interactable = true;
        canUnpause = true;
    }

    public void UnPauseGame(bool playSound)
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(isPaused);
        EventSystemInput.enabled = false;

        if (playSound) audioManager.Play("UISelect");
        StartCoroutine(setIgnoreInputAfterOneFrame());
    }

    public void Continue()
    {
        UnPauseGame(true);
    }

    private IEnumerator setIgnoreInputAfterOneFrame()
    {
        yield return null;
        ignoreInput = false;
    }

    public void RetryLevel()
    {
        UnPauseGame(true);

        LevelLoader.isDeath = true;
        RetryLevelEvent?.Invoke(this);
    }

    public void ReturnToTitle()
    {
        EventSystemInput.enabled = false;
        ExitButton.interactable = false;
        canUnpause = false;

        audioManager.Play("UISelect");

        LevelLoader.isDeath = false;
        ReturnToMenuEvent?.Invoke(this);

        CoinCollectScript.totalCoins = 0;
        AnimationScript.deaths = 0;
    }

    public void PlayButtonHoverSound()
    {
        audioManager.Play("UIMove");
    }

    private void ChangeRetryButton(bool isAscending)
    {
        Navigation topNav = new Navigation();
        Navigation bottomNav = new Navigation();

        topNav.mode = bottomNav.mode = Navigation.Mode.Explicit;

        if (isAscending)
        {
            RetryButton.interactable = false;
            RetryButtonText.color = retryDisabledColor;

            

            topNav.selectOnDown = ExitButton;
            topNav.selectOnUp = ExitButton;
            bottomNav.selectOnUp = ContinueButton;
            bottomNav.selectOnDown = ContinueButton;

        }
        else
        {
            RetryButton.interactable = true;
            RetryButtonText.color = defaultColor;

            topNav.selectOnDown = RetryButton;
            topNav.selectOnUp = ExitButton;
            bottomNav.selectOnUp = RetryButton;
            bottomNav.selectOnDown = ContinueButton;

        }

        ContinueButton.navigation = topNav;
        ExitButton.navigation = bottomNav;
    }
}
