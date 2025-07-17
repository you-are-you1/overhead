using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControlScript : MonoBehaviour
{
    public PlayerDataWithDash data;

    public InputSystem_Actions controls;
    InputAction cameraAction;

    private Vector2 cameraDirection;

    private GameObject cameraTarget;
    private CinemachinePositionComposer cineCameraComposer;
    private Bounds cineCameraConfinerBounds;

    private float cameraLookTimer;

    private bool isLooking;

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        cameraAction = controls.Player.Move;
        cameraAction.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        cameraAction.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTarget = transform.Find("Camera Follow Target").gameObject;
        CinemachineCamera c = FindAnyObjectByType<CinemachineCamera>();
        cineCameraComposer = c.GetComponent<CinemachinePositionComposer>();
        cineCameraConfinerBounds = c.GetComponent<CinemachineConfiner2D>().BoundingShape2D.bounds;

        cameraLookTimer = data.timeToLook;
    }

    // Update is called once per frame
    void Update()
    {
        cameraDirection = cameraAction.ReadValue<Vector2>().normalized;

        if (cameraDirection.y != 0)
        {
            cameraLookTimer -= Time.deltaTime;
            if (!isLooking && cameraLookTimer < 0)
            {
                isLooking = true;

                Vector3 targetPos = transform.position;
                targetPos.y = Camera.main.transform.position.y;
                //targetPos.y += data.distanceToLook * cameraDirection.y;

                targetPos.y += 2 * Camera.main.orthographicSize * cameraDirection.y;
                if (targetPos.y > cineCameraConfinerBounds.max.y - Camera.main.orthographicSize)
                {
                    targetPos.y = cineCameraConfinerBounds.max.y - Camera.main.orthographicSize;
                }
                else if (targetPos.y < cineCameraConfinerBounds.min.y + Camera.main.orthographicSize)
                {
                    targetPos.y = cineCameraConfinerBounds.min.y + Camera.main.orthographicSize; 
                }

                targetPos.y -= cineCameraComposer.TargetOffset.y;

                cameraTarget.transform.DOMove(targetPos, data.cameraMoveDuration).SetEase(Ease.OutSine);

                //float distanceBetweenCameraAndTarget = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
                //float cineCameraOffset = cineCameraComposer.TargetOffset.y;

                //if (cameraDirection.y > 0) cineCameraOffset *= -1;

                //cameraTarget.transform.position += Vector3.up * cameraDirection.y * (data.distanceToLook + distanceBetweenCameraAndTarget + cineCameraOffset);
            }
        }
        else
        {
            cameraLookTimer = data.timeToLook;
            if (isLooking)
            {
                isLooking = false;
                cameraTarget.transform.DOMove(transform.position, data.cameraMoveDuration).SetEase(Ease.OutSine);
            }
        }
    }
}
