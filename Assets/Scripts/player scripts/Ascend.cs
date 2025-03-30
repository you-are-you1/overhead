using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ascend : MonoBehaviour
{
    public PlayerDataWithDash Data;
    
    

    public InputSystem_Actions controls;
    InputAction abilityAction;

    private Rigidbody2D RB;
    private Collider2D playerCollider;

    private bool checkForAscend;
    private RaycastHit2D centerCheck;

    public Bounds bounds;
    private Collider2D overlap;

    public bool isAscending {  get; private set; }
    public bool isAscendBoosting;

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

        //enableItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (abilityAction.WasPressedThisFrame())
        {
            checkForAscend = true;
        }

        if (checkForAscend)
        {
            centerCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f),
                Vector2.up, Data.ascendRange, groundLayerMask);
            

            if (centerCheck)
            {
                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f), centerCheck.point);
            }
            else Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f),
                new Vector2(transform.position.x, transform.position.y + 0.5f + Data.ascendRange));

            
        }

        if (abilityAction.WasReleasedThisFrame() && !isAscending)
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

        while (!isAscendingInWall())
        {
            RB.linearVelocity = Vector2.up * Data.ascendSpeedOutsideWall;
            yield return null;
        }
        gameObject.layer = 8;
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

    private bool isAscendingInWall()
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
    }

    private void OnDisable()
    {
        abilityAction.Disable();
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
    
}
