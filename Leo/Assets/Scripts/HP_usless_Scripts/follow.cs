using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target; // Đối tượng để theo dõi (Player)
    public float smoothSpeed = 5f; // Tốc độ di chuyển camera
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Giữ camera phía sau

    void LateUpdate()
    {
        if (target == null) return; // Nếu không có target thì không làm gì

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
