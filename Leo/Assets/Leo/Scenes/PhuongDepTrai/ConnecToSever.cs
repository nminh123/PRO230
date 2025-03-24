using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Fin.Photon
{
    public class ConnecToSever : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject LoadingGUI;
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            LoadingGUI.SetActive(true);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }
        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            LoadingGUI.SetActive(false);
        }
    }
}
