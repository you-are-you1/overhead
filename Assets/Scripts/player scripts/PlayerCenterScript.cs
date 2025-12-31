using DG.Tweening;
using UnityEngine;

public class PlayerCenterScript : MonoBehaviour
{
   
    private PlayerMovementWithDash movement;
    private Rigidbody2D playerRB;

    public float movementMult;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = transform.parent.gameObject;
        movement = player.GetComponent<PlayerMovementWithDash>();
        playerRB = player.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = playerRB.linearVelocity * movementMult;
        if (!movement.IsFacingRight) target *= -1f;

        if (transform != null) transform.DOLocalMove(target, 0.2f);

        //transform.localPosition = playerRB.linearVelocity * movementMult;
        //if (!movement.IsFacingRight) transform.localPosition *= -1f;
    }

    private void OnDisable()
    {
        DOTween.Clear();
    }
}
