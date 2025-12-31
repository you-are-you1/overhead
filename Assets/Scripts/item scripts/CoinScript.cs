using System;
using System.Collections;
using System.Linq.Expressions;
using DG.Tweening;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public static event Action<CoinScript> OnCollectCoinEvent;

    private GameObject player;
    private Animator animator;

    private ParticleSystem coinPS;

    private AudioSource audioSource;

    [SerializeField] private float loopTime;
    [SerializeField] private float loopDistance;
    [SerializeField] private float shineTime;

    [SerializeField] private float collectMoveTime;
    [SerializeField] private float collectMoveDistance;

    [SerializeField] private float collectAnimTime;

    private bool isCollected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isCollected = false;
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();

        coinPS = GetComponent<ParticleSystem>();

        audioSource = GetComponent<AudioSource>();

        transform.position -= new Vector3(0f, loopDistance * 0.5f, 0f);
        transform.DOMove(transform.position + new Vector3(0f, loopDistance, 0f), loopTime)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        StartCoroutine(doShine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator doShine()
    {
        while (!isCollected)
        {
            yield return new WaitForSeconds(shineTime);

            animator.SetTrigger("Shine");
        }
    }

    private IEnumerator doCollect()
    {
        DOTween.Clear();
        transform.DOMove(transform.position + new Vector3(0f, collectMoveDistance, 0f), collectMoveTime).SetEase(Ease.OutSine);
        audioSource.Play();
        yield return new WaitForSeconds(collectMoveTime);

        animator.SetTrigger("Collect");

        

        yield return new WaitForSeconds(collectAnimTime);
        
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && !isCollected)
        {
            StopCoroutine(doShine());
            isCollected = true;
            OnCollectCoinEvent?.Invoke(this);
            coinPS.Stop();
            StartCoroutine(doCollect());
            
        } 
    }
}
