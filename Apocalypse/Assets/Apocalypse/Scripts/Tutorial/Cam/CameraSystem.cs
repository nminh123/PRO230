using Cinemachine;
using UnityEngine;

public class CameraSystem : Singleton<CameraSystem>
{
    [Header("References")]
    public CinemachineVirtualCamera virtualCam;
    public Transform player;

    [Header("Camera Settings")]
    public float moveSpeed = 10f;
    public float edgeSize = 10f; 

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minOrthoSize = 5f;
    public float maxOrthoSize = 20f;
    public float mouseZoomSpeed = 2f; 

    private bool isCameraLocked = true;
    private Vector3 manualCamPos;
    private float targetOrthoSize;

    private void Start()
    {
        if (virtualCam == null)
            virtualCam = GetComponent<CinemachineVirtualCamera>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        targetOrthoSize = virtualCam.m_Lens.OrthographicSize;

        LockCamera(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCameraLocked = !isCameraLocked;
            LockCamera(isCameraLocked);
        }

        if (!isCameraLocked)
        {
            //HandleKeyboardMovement();
            HandleEdgeScroll();
        }

        ZoomInput();
        ApplyZoom();
    }

    protected void ZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            targetOrthoSize -= scroll * zoomSpeed;
            targetOrthoSize = Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize);
        }

        if (Input.GetMouseButton(2))
        {
            float middleMouseScroll = Input.GetAxis("Mouse Y");
            targetOrthoSize += middleMouseScroll * mouseZoomSpeed;
            targetOrthoSize = Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize);
        }
    }
    // setup zoom camera
    protected void ApplyZoom()
    {
        if (!Mathf.Approximately(virtualCam.m_Lens.OrthographicSize, targetOrthoSize))
        {
            virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCam.m_Lens.OrthographicSize,
                targetOrthoSize,
                Time.deltaTime * zoomSpeed
            );
        }
    }

    // Khoa va mo camera
    protected void LockCamera(bool isLock)
    {
        if (isLock)
        {
            virtualCam.Follow = player;
            // Reset zoom to default when locking?
            // targetOrthoSize = defaultOrthoSize;
        }
        else
        {
            virtualCam.Follow = null;
            manualCamPos = virtualCam.transform.position;
        }
    }

    protected void HandleKeyboardMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;
        manualCamPos += move;
        virtualCam.transform.position = manualCamPos;
    }

    protected void HandleEdgeScroll()
    {
        Vector3 move = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x >= Screen.width - edgeSize)
            move.x += 1;
        else if (mousePos.x <= edgeSize)
            move.x -= 1;

        if (mousePos.y >= Screen.height - edgeSize)
            move.y += 1;
        else if (mousePos.y <= edgeSize)
            move.y -= 1;

        if (move != Vector3.zero)
        {
            manualCamPos += move.normalized * moveSpeed * Time.deltaTime;
            virtualCam.transform.position = manualCamPos;
        }
    }
}