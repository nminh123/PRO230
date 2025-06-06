using UnityEngine;

namespace Game.Tutorial.Turret
{
    public class UpgradeTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject upgradePopUp;

        private void OnEnable()
        {
            Events.Instance.PopupEnableEvent += EnablePopUp;
        }

        private void OnDisable()
        {
            Events.Instance.PopupEnableEvent -= EnablePopUp;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tag.PLAYER))
            {
                Debug.Log($"Enter - Object trigger tag {Tag.PLAYER}");
                // Events.Instance.InvokeCheckHotBarEvent();
                TowerUpgrade.Instance.IsTrigger = true;
                // Events.Instance.InvokePopupEnableEvent();
                Events.Instance.InvokeWarningIconEvent(true);
                // upgradePopUp.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tag.PLAYER))
            {
                Debug.Log($"Exit - Object trigger tag {Tag.PLAYER}");
                TowerUpgrade.Instance.IsTrigger = false;
                Events.Instance.InvokeWarningIconEvent(false);
                upgradePopUp.SetActive(false);
            }
        }

        private void EnablePopUp()
        {
            upgradePopUp.SetActive(true);
        }
    }
}