using UnityEngine;

public class SwitchSpringScript : MonoBehaviour
{
    [SerializeField] private bool isSolid;
    

    private SpriteRenderer SpriteRenderer;
    private BoxCollider2D springCollider;

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        springCollider = GetComponent<BoxCollider2D>();
        springCollider.enabled = isSolid;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchSpring(DottedTilemapScript d)
    {
        isSolid = !isSolid;
        springCollider.enabled = isSolid;

        animator.SetTrigger("Switch");

        //if (isSolid)
        //{
        //    SpriteRenderer.sprite = solidSprite;
        //}
        //else
        //{
        //    SpriteRenderer.sprite = dottedSprite;
        //}

    }

    private void OnEnable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent += SwitchSpring;
    }

    private void OnDisable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent -= SwitchSpring;
    }
}
