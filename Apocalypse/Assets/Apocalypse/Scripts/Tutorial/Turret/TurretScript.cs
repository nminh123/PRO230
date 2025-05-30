using Game.Tutorials;
using UnityEngine;

namespace Game.Tutorial.Turret
{
    public class TurretScript : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable() => Events.Instance.UpgradeEvent += UpdateSprite;

        private void Start()
        {
            spriteRenderer.sprite = TowerUpgrade.Instance.GetTowerSO.GetSprite;
        }

        private void UpdateSprite()
        {
            spriteRenderer.sprite = TowerUpgrade.Instance.GetTowerSO.GetSprite;
        }

        private void OnDisable() => Events.Instance.UpgradeEvent += UpdateSprite;
    }
}