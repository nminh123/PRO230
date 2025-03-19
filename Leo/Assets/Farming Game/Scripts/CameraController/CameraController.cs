using UnityEngine;
using FarmingGame.Player;

namespace FarmingGame.CameraController
{
    //Có thể sửa class này!!.
    public class CameraController : MonoBehaviour
    {
        private Transform target;
        public Transform clampMin, clampMax;

        void Start()
        {
            target = FindObjectOfType<PlayerController>().transform;
            clampMin.SetParent(null);
            clampMax.SetParent(null);
        }

        void Update()
        {
            FollowedTarget();
            ClampCamera();
        }

        void FollowedTarget()
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

        void ClampCamera()
        {
            Vector3 clampedPosition = transform.position;

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, clampMin.position.x, clampMax.position.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, clampMin.position.y, clampMax.position.y);

            transform.position = clampedPosition;
        }
    }
}