using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tutorial
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected Image fill;

        //[SerializeField] private GameObject damageTextPrefab;
        //[SerializeField] private Transform damageTextSpawnPoint;
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            UpdateHealthBar();
        }

        public void TakeDamage(float damage)
        {
            if (currentHealth <= 0) return;
            currentHealth = Mathf.Max(0, currentHealth - damage);
            UpdateHealthBar();
        }

        public void AddHealth(float health)
        {
            if (currentHealth >= maxHealth) return;
            currentHealth = Mathf.Min(maxHealth, currentHealth + health);
            UpdateHealthBar();
        }

        #region GUI
        protected void UpdateHealthBar()
        {
            float healthRatio = currentHealth / maxHealth;
            StartCoroutine(SmoothHealthBar(healthRatio));

            fill.color = (healthRatio < 0.3f)
                ? Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 2, 1))
                : Color.green;
        }

        protected IEnumerator SmoothHealthBar(float targetFill)
        {
            float startFill = fill.fillAmount;
            float elapsedTime = 0f;
            float duration = 0.5f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                fill.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime / duration);
                yield return null;
            }

            fill.fillAmount = targetFill;
        }
        //void ShowDamageText(int damage)
        //{
        //    Vector3 spawnPosition = damageTextSpawnPoint.position;
        //    photonView.RPC("ShowDamageTextRPC", RpcTarget.All, damage, spawnPosition);
        //}
        //[PunRPC]
        //void ShowDamageTextRPC(int damage, Vector3 position)
        //{
        //    if (damageTextPrefab == null) return;

        //    GameObject dmgTextObj = Instantiate(damageTextPrefab, position, Quaternion.identity, damageTextSpawnPoint.parent);
        //    DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        //    if (dmgText != null)
        //    {
        //        dmgText.ShowDamage(damage);
        //    }
        //}
        #endregion
    }

}