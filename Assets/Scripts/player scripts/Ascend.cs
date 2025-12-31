using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ascend : MonoBehaviour
{
    public PlayerDataWithDash Data;

    public static event Action<Ascend> NextLevelEvent;
    public static event Action<Ascend> StartGameplayEvent;
    

    public InputSystem_Actions controls;
    public InputAction abilityAction;

    private Rigidbody2D RB;
    private Collider2D playerCollider;
    private Animator animator;
    private SpriteMask mask;

    private LineDrawScript lds;

    private float ascendCooldown;
    private bool canStart;

    public bool checkForAscend {  get; set; }
    public RaycastHit2D centerCheck {  get; private set; }
    public RaycastHit2D leftCheck { get; private set; }
    public RaycastHit2D rightCheck { get; private set; }

    [HideInInspector] public Bounds bounds;
    private Collider2D overlap;

    public bool isAscending {  get; private set; }
    [HideInInspector] public bool isAscendBoosting;

    public float jumpAfterAscendTimer { get; private set; }

    [SerializeField] private GameObject levelTrigger;
    private bool isInLevelTrigger;

    int groundLayerMask;

    public float sideCheckOffset;
    public float waitTimeForControls;

    public GameObject effect;
    public float effectFrequency;

    [Header("Particles")]
    public GameObject ascendPS;
    public GameObject ascendBoostPS;

    private AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityAction = InputSystem.actions.FindAction("Ability");
        checkForAscend = false;
        isAscending = false;
        isAscendBoosting = false;

        canStart = false;

        animator = GetComponent<Animator>();

        mask = GetComponent<SpriteMask>();

        lds = GetComponent<LineDrawScript>();

        audioManager = AudioManager.instance;

        groundLayerMask = 1 << 6; //layerMask with only ground tiles

        bounds = new Bounds();
        overlap = new Collider2D();

        ascendCooldown = 0;
        jumpAfterAscendTimer = 0;
        //enableItem();

        

        StartCoroutine(waitForControls());
    }

    // Update is called once per frame
    void Update()
    {
        ascendCooldown -= Time.deltaTime;
        jumpAfterAscendTimer -= Time.deltaTime;
        

        if (abilityAction.WasPressedThisFrame() && !isAscending && canStart && !PauseScript.ignoreInput)
        {
            if (isInLevelTrigger)
            {
                NextLevelEvent?.Invoke(this);
                PauseScript.canPause = false;
                audioManager.Play("LevelEnd");
                abilityAction.Disable();
            }
            else if (ascendCooldown < 0)
            {
                checkForAscend = true;
                audioManager.Play("AscendCheck", 0.5f, 1.5f);
            }
        }

        
        if (checkForAscend)
        {
            centerCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f),
                Vector2.up, Data.ascendRange, groundLayerMask);
            leftCheck = Physics2D.Raycast(new Vector2(transform.position.x - sideCheckOffset, transform.position.y + 0.5f),
                Vector2.up, Data.ascendRange, groundLayerMask);
            rightCheck = Physics2D.Raycast(new Vector2(transform.position.x + sideCheckOffset, transform.position.y + 0.5f),
                Vector2.up, Data.ascendRange, groundLayerMask);


            //if (centerCheck)
            //{
            //    Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f), centerCheck.point);
            //}
            //else Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f),
            //    new Vector2(transform.position.x, transform.position.y + 0.5f + Data.ascendRange));


        }

        if (abilityAction.WasReleasedThisFrame() && !isAscending && checkForAscend && !PauseScript.ignoreInput)
        {
            checkForAscend = false;

            if (centerCheck)
            {
                Sleep(Data.ascendSleepTime);
                StartCoroutine(nameof(StartAscend));
            }

        }




    }

    private IEnumerator StartAscend()
    {
        float startY = transform.position.y;
     
        bool didNotReachWall = true;
        isAscending = true;
        isAscendBoosting = false;

        float timeSum = 0;
        
        SetGravityScale(0);

        gameObject.layer = 8;

        lds.enableTrails();

        audioManager.Play("AscendStart", 0.5f, 1.5f);

        while (!isAscendingInWall() && startY + Data.ascendRange > transform.position.y)
        {
            RB.linearVelocity = Vector2.up * Data.ascendSpeedOutsideWall;
            yield return null;
        }

        lds.disableTrails();

        if (isAscendingInWall())
        {
            Sleep(Data.ascendSleepBetweenTime);
            didNotReachWall = false;
            animator.SetTrigger("ascend");
            mask.enabled = true;
            
            Instantiate(ascendPS, transform.position, Quaternion.identity);
        }

        while (isAscendingInWall())
        {
            RB.linearVelocity = Vector2.up * Data.ascendSpeedInWall;

            timeSum += Time.deltaTime;
            if (timeSum >= effectFrequency)
            {
                Instantiate(effect, transform.position, Quaternion.identity);

                timeSum = 0;
            }

            yield return null;
        }

        animator.SetTrigger("stop ascend");
        mask.enabled = false;
        

        gameObject.layer = 0;
        RB.linearVelocity = didNotReachWall? Vector2.up * Data.ascendEndBoostNoWall : Vector2.up * Data.ascendEndBoost;
        SetGravityScale(Data.gravityScale);

        isAscending = false;
        isAscendBoosting = true;
        jumpAfterAscendTimer = Data.jumpPreventionAfterAscendTime;

        audioManager.Play("AscendBoost", 0.5f, 1f);

        Instantiate(ascendBoostPS, transform.position, Quaternion.identity);
    }

    private IEnumerator waitForControls()
    {
        yield return new WaitForSeconds(waitTimeForControls);
        abilityAction = controls.Player.Ability;
        abilityAction.Enable();

        StartGameplayEvent?.Invoke(this);

        canStart = true;
        PauseScript.canPause = true;
    }

    public bool isAscendingInWall()
    {
        bounds = playerCollider.bounds;

        overlap = Physics2D.OverlapBox(new Vector3(bounds.center.x, bounds.center.y + 0.1f, 0f), bounds.size - new Vector3(0.1f, 0.1f, 0f), 0, groundLayerMask);
        

        return overlap != null;
    }
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
       
        
        controls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
       
        SpikesScript.OnPlayerTouchSpikesEvent += stopAscendSpikes;
        AscendBlockerScript.OnPlayerTouchAscendBlocker += stopAscendBlocker;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent += disableControls;

    }

    private void OnDisable()
    {
        controls.Disable();
        abilityAction.Disable();
        SpikesScript.OnPlayerTouchSpikesEvent -= stopAscendSpikes;
        AscendBlockerScript.OnPlayerTouchAscendBlocker -= stopAscendBlocker;
        DeathPlaneScript.OnPlayerTouchDeathPlaneEvent -= disableControls;
    }

    private void Sleep(float duration)
    {
        //Method used so we don't need to call StartCoroutine everywhere
        //nameof() notation means we don't need to input a string directly.
        //Removes chance of spelling mistakes and will improve error messages if any
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
        if (!PauseScript.isPaused) Time.timeScale = 1;

    }

    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    private void stopAscendSpikes(SpikesScript script)
    {
        stopAscend();
    }

    private void stopAscendBlocker(AscendBlockerScript blocker)
    {
        stopAscend();
    }

    private void disableControls(DeathPlaneScript script)
    {
        controls.Disable();
    }

    private void stopAscend()
    {
        if (isAscending)
        {
            StopCoroutine(nameof(StartAscend));
            gameObject.layer = 0;
            SetGravityScale(Data.gravityScale);
            isAscending = false;
            ascendCooldown = Data.ascendBlockerCooldownTime;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == levelTrigger)
        {
            isInLevelTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == levelTrigger)
        {
            isInLevelTrigger = false;
        }
    }

}
