using UnityEngine;

namespace Game.Tutorial.Turret
{
    public class TowerPointer : MonoBehaviour
    {
        private void Update()
        {
            if (TowerUpgrade.Instance.IsTrigger)
                HandleInput();
        }

        private void HandleInput()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CheckHit(worldPoint);
            }
#else
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                CheckHit(worldPoint);
            }
#endif
        }

        private void CheckHit(Vector2 worldPoint)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                // if (TowerUpgrade.Instance.isReadyToUpgrade)
                // {
                //     Debug.Log("Hit object: " + hit.collider.name);
                //     // TowerUpgrade.Instance.IteratorHotbar(TowerUpgrade.Instance.cpyRequirements);
                //     Events.Instance.InvokePopupEnableEvent();
                // }
                Debug.Log("Hit object: " + hit.collider.name);
                // TowerUpgrade.Instance.IteratorHotbar(TowerUpgrade.Instance.cpyRequirements);
                Events.Instance.InvokePopupEnableEvent();
            }
        }
    }
}