using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraOffsetScript : MonoBehaviour
{
    private GameObject player;
    private CinemachinePositionComposer cineCameraComposer;

    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 damping = new Vector2(1, 1);
    [SerializeField] private float duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cineCameraComposer = FindAnyObjectByType<CinemachineCamera>().GetComponent<CinemachinePositionComposer>();
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
}
