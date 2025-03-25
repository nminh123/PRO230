using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

namespace Fin.Photon
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField InputCreateRoom;
        [SerializeField]private TMP_InputField InputJoinRoom;

        [SerializeField] private Button createRoomBtn;
        [SerializeField] private Button joinRoomBtn;

        [SerializeField] private GameObject roomPrefab;
        [SerializeField] private Transform roomParent;
        private Dictionary<string, GameObject> currentRoomUIs = new Dictionary<string, GameObject>();
        private void Start()
        {
            createRoomBtn.onClick.AddListener(CreateRoom);
            joinRoomBtn.onClick.AddListener(JoinRoom);
        }
        private void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(InputCreateRoom.text,roomOptions);
        }
        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(InputJoinRoom.text);
        }
        public void JoinRoomByName(string nameRoom)
        {
            PhotonNetwork.JoinRoom(nameRoom);
        }
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Map1");
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (Transform child in roomParent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (RoomInfo room in roomList)
            {
                if (room.IsOpen && room.IsVisible && room.PlayerCount > 0)
                {
                    GameObject newRoom = Instantiate(roomPrefab, roomParent.transform);
                    newRoom.GetComponentInChildren<TextMeshProUGUI>().text = room.Name;
                }
            }
        }
    }
}
