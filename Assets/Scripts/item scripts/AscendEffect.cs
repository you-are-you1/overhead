using System.Collections;
using DG.Tweening;
using UnityEngine;

public class AscendEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteMask mask;

    public float sleepTime;
    public float stayTime;
    public float fadeTime;
    public float maskAnimationTime;

    public Sprite[] maskSprites;

    private bool hasActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!hasActivated)
        {

            hasActivated = true;

            
            StartCoroutine(fadeOut());
        }
    }

    private IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(stayTime);

        spriteRenderer.DOFade(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        for (int i = 0; i < maskSprites.Length; i++)
        {
            mask.sprite = maskSprites[i];
            yield return new WaitForSeconds(maskAnimationTime);
        }
        Destroy(gameObject);
    }


}
