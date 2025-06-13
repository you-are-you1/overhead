using Unity.Cinemachine;
using UnityEngine;

public class CameraOffsetScript : MonoBehaviour
{
    private GameObject player;
    private CinemachinePositionComposer cineCameraComposer;

    [SerializeField] private Vector2 offset;
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
            cineCameraComposer.TargetOffset = offset;
            Debug.Log(offset);
        }
    }
}
