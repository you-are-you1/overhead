using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;


public class RespawnAnimScript : MonoBehaviour
{
    public float minStart;
    public float maxStart;
    public float minTransitionStart;
    public float maxTransitionStart;
    public float spawnTime;
    public float transitionTime;
    public float respawnWaitTime;
    public float levelTransitionWaitTime;

    private List<GameObject> effects = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Respawn Effect"))
            {
                effects.Add(child.gameObject);
            }
        }

        StartCoroutine(respawnEffect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void doTransitionEffect(Ascend a)
    {
        StartCoroutine(levelTransitionEffect());
    }

    private IEnumerator respawnEffect()
    {
        yield return new WaitForSeconds(respawnWaitTime);

        AudioManager.instance.Play("Respawn");

        foreach (GameObject g in effects)
        {
           
            g.transform.localPosition += new Vector3(0f, Random.Range(minStart, maxStart), 0f);
            g.transform.DOLocalMove(new Vector3(g.transform.localPosition.x, 0f, 0f), spawnTime).SetEase(Ease.OutCubic);
        }


    }

    private IEnumerator levelTransitionEffect()
    {
        yield return new WaitForSeconds(levelTransitionWaitTime);

        foreach (GameObject g in effects)
        {
            Vector3 target = new Vector3(g.transform.localPosition.x, Random.Range(minTransitionStart, maxTransitionStart), 0f);
            g.transform.DOLocalMove(target, transitionTime).SetEase(Ease.InCubic);
        }
    }

    private void OnEnable()
    {
        Ascend.NextLevelEvent += doTransitionEffect;
    }

    private void OnDisable()
    {
        Ascend.NextLevelEvent -= doTransitionEffect;
    }
}
