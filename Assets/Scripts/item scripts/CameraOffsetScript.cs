using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraOffsetScript : MonoBehaviour
{
    private GameObject player;
    private CinemachinePositionComposer cineCameraComposer;

    private BoxCollider2D boxCollider;

    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 damping = new Vector2(1, 1);
    [SerializeField] private float duration;
    [SerializeField] private bool onlyActivateAfterTileSwitch;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cineCameraComposer = FindAnyObjectByType<CinemachineCamera>().GetComponent<CinemachinePositionComposer>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (onlyActivateAfterTileSwitch) boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            DOTween.To(() => (Vector2)cineCameraComposer.TargetOffset,
                x => cineCameraComposer.TargetOffset = x, offset, duration).SetEase(Ease.OutQuad);
            //cineCameraComposer.TargetOffset = offset;
            cineCameraComposer.Damping = damping;
            
        }
    }
    private void enableBoxCollider(DottedTilemapScript d)
    {
        boxCollider.enabled = true;
    }

    private void OnEnable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent += enableBoxCollider;
    }

    private void OnDisable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent -= enableBoxCollider;
    }
}
