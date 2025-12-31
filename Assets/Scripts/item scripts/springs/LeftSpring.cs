using UnityEngine;

public class LeftSpring : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private PlayerMovementWithDash movement;
    private Ascend playerAscend;
    private Vector2 springForce;

    private Animator animator;
    private ParticleSystem ps;

    public SpringData springData;
    public PlayerDataWithDash playerData;

    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
        movement = player.GetComponent<PlayerMovementWithDash>();
        playerAscend = player.GetComponent<Ascend>();
        springForce = springData.leftSpringForce;

        animator = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player && !playerAscend.isAscending)
        {
            playerAscend.isAscendBoosting = false;

            movement.isSpringBoosting = true;
            movement.SpringBoostTimer = springData.SpringBoostCheckDuration;
            movement.SideSpringLerpTimer = playerData.timeToLerp;
            //movement.LastOnWallTime = 0;
            //movement.LastOnWallLeftTime = 0;
            //movement.LastOnWallRightTime = 0;

            rb.gravityScale = playerData.gravityScale * playerData.fallGravityMult;
            rb.linearVelocity = new Vector2(springForce.x, springForce.y);

            animator.SetTrigger("Bounce");

            ps.Play();

            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(audioSource.clip);
        }

    }
}
