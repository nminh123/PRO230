using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
namespace Fin.Photon
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private float currentHealth;
        [SerializeField] private float maxHealth;
        [SerializeField] private Image fill;

        private void Start()
        {
            currentHealth = maxHealth;
            UpdateHealthBar();
        }

        private void Update()
        {
            if (!photonView.IsMine) return;

            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(10);
            }
        }

        public void TakeDamage(float damage)
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage);
            }
        }

        [PunRPC]
        public void RPC_TakeDamage(float damage)
        {
            currentHealth -= damage;
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            float healthPercentage = currentHealth / maxHealth;
            StartCoroutine(SmoothHealthBar(healthPercentage));

            if (healthPercentage < 0.3f)
                fill.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 2, 1));
            else
                fill.color = Color.green;
        }
        private IEnumerator SmoothHealthBar(float targetFill)
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
    }
}
