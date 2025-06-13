using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ascend : MonoBehaviour
{
    public PlayerDataWithDash Data;

    public static event Action<Ascend> NextLevelEvent;

    public InputSystem_Actions controls;
    InputAction abilityAction;

    private Rigidbody2D RB;
    private Collider2D playerCollider;

    private float ascendCooldown;

    public bool checkForAscend {  get; private set; }
    public RaycastHit2D centerCheck {  get; private set; }

    [HideInInspector] public Bounds bounds;
    private Collider2D overlap;

    public bool isAscending {  get; private set; }
    [HideInInspector] public bool isAscendBoosting;

    [SerializeField] private GameObject levelTrigger;
    private bool isInLevelTrigger;

    int groundLayerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityAction = InputSystem.actions.FindAction("Ability");
        checkForAscend = false;
        isAscending = false;
        isAscendBoosting = false;

        groundLayerMask = 1 << 6;

        bounds = new Bounds();
        overlap = new Collider2D();

        ascendCooldown = 0;
        //enableItem();
    }

    // Update is called once per frame
    void Update()
    {
        ascendCooldown -= Time.deltaTime;

        if (abilityAction.WasPressedThisFrame() && !isAscending)
        {
            if (isInLevelTrigger)
            {
                NextLevelEvent?.Invoke(this);
            }
            else if (ascendCooldown < 0)
            {
                checkForAscend = true;
            }
        }

        if (checkForAscend)
        {
            centerCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f),
                Vector2.up, Data.ascendRange, groundLayerMask);
            

            //if (centerCheck)
            //{
            //    Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f), centerCheck.point);
            //}
            //else Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f),
            //    new Vector2(transform.position.x, transform.position.y + 0.5f + Data.ascendRange));

            
        }

        if (abilityAction.WasReleasedThisFrame() && !isAscending && checkForAscend)
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
        isAscending = true;
        isAscendBoosting = false;
        
        SetGravityScale(0);

        gameObject.layer = 8;

        while (!isAscendingInWall())
        {
            RB.linearVelocity = Vector2.up * Data.ascendSpeedOutsideWall;
            yield return null;
        }
        
        Sleep(Data.ascendSleepBetweenTime);   

        while (isAscendingInWall())
        {
            RB.linearVelocity = Vector2.up * Data.ascendSpeedInWall;
            yield return null;
        }

        gameObject.layer = 0;
        RB.linearVelocity = Vector2.up * Data.ascendEndBoost;
        SetGravityScale(Data.gravityScale);

        isAscending = false;
        isAscendBoosting = true;
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
        abilityAction = controls.Player.Ability;
        abilityAction.Enable();
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
        Time.timeScale = 1;

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
            ascendCooldown = Data.ascendCooldownTime;
            Debug.Log("stoped");
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
