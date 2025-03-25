using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fin.Photon
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] protected PhotonView photonView;
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected Image fill;

        public float CurrentHealth => currentHealth;

        protected virtual void Start()
        {
            currentHealth = maxHealth;
        }
        public void TakeDamage(float damage)
        {
            if (!photonView.IsMine) return;
            photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage);

        }

        [PunRPC]
        public void RPC_TakeDamage(float damage)
        {
            if(currentHealth <= 0) return;
            currentHealth -= damage;
            UpdateHealthBar();
        }
        public void AddHealth(float health)
        {
            if(!photonView.IsMine) return;
            photonView.RPC(nameof(RPC_AddHealth), RpcTarget.All, health);
        }
        [PunRPC]
        public void RPC_AddHealth(float health)
        {
            if(currentHealth >= 100) return;
            currentHealth += health;
            UpdateHealthBar();
        }
        #region GUI
        protected void UpdateHealthBar()
        {
            float health = currentHealth / maxHealth;
            StartCoroutine(SmoothHealthBar(health));

            if (health < 0.3f)
                fill.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 2, 1));
            else
                fill.color = Color.green;
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
        #endregion
    }

}