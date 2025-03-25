using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Fin.Photon
{
    public class SpawnPlayer : MonoBehaviour
    {
        private void Start()
        {
            Vector2 posSpawn = new Vector2(Random.Range(-2, 2), 0);
            GameObject player = PhotonNetwork.Instantiate("Player", posSpawn, Quaternion.identity);
        }
    }

}