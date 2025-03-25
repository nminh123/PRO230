using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
namespace Fin.Photon
{
    public class PlayerStats : Stats
    {
        protected override void Start()
        {
            base.Start();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(10);
            }
        }
    }
}
