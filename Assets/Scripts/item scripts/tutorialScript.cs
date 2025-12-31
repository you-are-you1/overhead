using System.Collections;
using DG.Tweening;
using UnityEngine;

public class tutorialScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private GameObject upperMask;
    private SpriteRenderer upperLine;
    private GameObject lowerMask;
    private SpriteRenderer lowerLine;

    private GameObject leftMask, rightMask;

    private float spriteX, spriteY;

    public float verticalTime, horizontalTime;
    public float addedLineWidth;

    private float lineWidth;

    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        spriteRenderer = GetComponent<SpriteRenderer>();

        upperMask = transform.Find("upperMask").gameObject;
        upperLine = transform.Find("upperLine").GetComponent<SpriteRenderer>();
        lowerMask = transform.Find("lowerMask").gameObject;
        lowerLine = transform.Find("lowerLine").GetComponent<SpriteRenderer>();

        leftMask = transform.Find("leftMask").gameObject;
        rightMask = transform.Find("rightMask").gameObject;

        spriteX = spriteRenderer.sprite.bounds.size.x;
        spriteY = spriteRenderer.sprite.bounds.size.y;

        Vector3 verticalMaskSize = new Vector3(spriteX, spriteY * 0.5f, 1f);

        upperMask.transform.localScale = verticalMaskSize;
        lowerMask.transform.localScale = verticalMaskSize;

        lineWidth = spriteX + addedLineWidth;

        upperLine.size = new Vector2(lineWidth, upperLine.size.y);
        lowerLine.size = new Vector2(lineWidth, lowerLine.size.y);

        leftMask.transform.localScale = new Vector3(lineWidth * 0.5f, 1f, 1f);
        rightMask.transform.localScale = new Vector3(lineWidth * 0.5f, 1f, 1f);



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Open()
    {
        leftMask.transform.DOLocalMoveX(-lineWidth * 0.5f, horizontalTime).SetEase(Ease.OutQuad);
        rightMask.transform.DOLocalMoveX(lineWidth * 0.5f, horizontalTime).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(horizontalTime);

        upperMask.transform.DOLocalMoveY(spriteY * 0.5f, verticalTime).SetEase(Ease.OutQuad);
        upperLine.transform.DOLocalMoveY(spriteY * 0.5f, verticalTime).SetEase(Ease.OutQuad);
        lowerMask.transform.DOLocalMoveY(-spriteY * 0.5f, verticalTime).SetEase(Ease.OutQuad);
        lowerLine.transform.DOLocalMoveY(-spriteY * 0.5f, verticalTime).SetEase(Ease.OutQuad);
    }

    private IEnumerator Close()
    {
        upperMask.transform.DOLocalMoveY(0f, verticalTime).SetEase(Ease.OutQuad);
        upperLine.transform.DOLocalMoveY(0f, verticalTime).SetEase(Ease.OutQuad);
        lowerMask.transform.DOLocalMoveY(0f, verticalTime).SetEase(Ease.OutQuad);
        lowerLine.transform.DOLocalMoveY(0f, verticalTime).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(verticalTime);

        leftMask.transform.DOLocalMoveX(0f, horizontalTime).SetEase(Ease.OutQuad);
        rightMask.transform.DOLocalMoveX(0f, horizontalTime).SetEase(Ease.OutQuad);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            StopAllCoroutines();
            StartCoroutine(Open());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            StopAllCoroutines();
            StartCoroutine(Close());
        }
    }
}
