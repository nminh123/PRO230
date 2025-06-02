using Cinemachine;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera virtualCam;
    public Transform player;

    [Header("Camera Settings")]
    public float moveSpeed = 10f;
    public float edgeSize = 10f; // Pixels từ rìa màn hình

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minOrthoSize = 5f;
    public float maxOrthoSize = 20f;
    public float middleMouseZoomSpeed = 2f; // Tốc độ zoom khi dùng chuột giữa

    private bool isCameraLocked = true;
    private Vector3 manualCamPos;
    private float targetOrthoSize;

    void Start()
    {
        if (virtualCam == null)
            virtualCam = GetComponent<CinemachineVirtualCamera>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        targetOrthoSize = virtualCam.m_Lens.OrthographicSize;

        LockCamera(true);
    }

    void Update()
    {
        // Toggle lock/unlock camera
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

        // Handle zoom input
        HandleZoomInput();

        // Smoothly apply zoom
        ApplyZoom();
    }

    void HandleZoomInput()
    {
        // Normal mouse wheel zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            targetOrthoSize -= scroll * zoomSpeed;
            targetOrthoSize = Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize);
        }

        // Middle mouse button drag zoom (like League of Legends)
        if (Input.GetMouseButton(2)) // Chuột giữa
        {
            float middleMouseScroll = Input.GetAxis("Mouse Y");
            targetOrthoSize += middleMouseScroll * middleMouseZoomSpeed;
            targetOrthoSize = Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize);
        }
    }

    void ApplyZoom()
    {
        // Smoothly interpolate to target zoom
        if (!Mathf.Approximately(virtualCam.m_Lens.OrthographicSize, targetOrthoSize))
        {
            virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCam.m_Lens.OrthographicSize,
                targetOrthoSize,
                Time.deltaTime * zoomSpeed
            );
        }
    }

    void LockCamera(bool shouldLock)
    {
        if (shouldLock)
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

    void HandleKeyboardMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;
        manualCamPos += move;
        virtualCam.transform.position = manualCamPos;
    }

    void HandleEdgeScroll()
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