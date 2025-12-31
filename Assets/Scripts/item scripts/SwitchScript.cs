using System;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    private GameObject player;
    private Animator switchAnimator;
    private AudioSource audioSource;
    private bool collected;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem particles;
    private ParticleSystem centrePS;
    private ParticleSystemRenderer centrePSRenderer;

    [SerializeField] private float particleBorder;

    public static event Action<SwitchScript> OnSwitchCollectEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        switchAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        collected = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
        centrePS = transform.GetChild(0).GetComponent<ParticleSystem>();
        centrePSRenderer = centrePS.GetComponent<ParticleSystemRenderer>();

        var sh = particles.shape;

        sh.scale = spriteRenderer.size - new Vector2(particleBorder, particleBorder);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && !collected)
        {
            collected = true;
            switchAnimator.SetTrigger("Collected");
            particles.Stop();
            centrePSRenderer.enabled = true;
            centrePS.Play();
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.Play();
            OnSwitchCollectEvent?.Invoke(this);
        }
    }
}
