using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fin.Photon
{
    public class PlayerTrigger : MonoBehaviour
    {
        //[SerializeField] PhotonView photonView;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hitTarget = collision.GetComponent<Stats>();
            if (hitTarget)
            {
                hitTarget.TakeDamage(10);
                Debug.Log("a");
            }
    }
    }
}
