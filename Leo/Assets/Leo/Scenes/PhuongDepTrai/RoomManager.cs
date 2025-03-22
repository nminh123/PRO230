using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Fin.Photon
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField InputCreateRoom;
        [SerializeField]private TMP_InputField InputJoinRoom;

        [SerializeField] private Button createRoomBtn;
        [SerializeField] private Button joinRoomBtn;
        private void Start()
        {
            createRoomBtn.onClick.AddListener(CreateRoom);
            joinRoomBtn.onClick.AddListener(JoinRoom);
        }

        private void CreateRoom()
        {
            PhotonNetwork.CreateRoom(InputCreateRoom.text);
        }
        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(InputJoinRoom.text);
        }
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Map1");
        }
    }
}
