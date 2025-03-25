using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
namespace Fin.Photon
{
    public class Room : MonoBehaviour
    {
        public TextMeshProUGUI textRoom;
        public Button joinRoomButton;

        private void Start()
        {
            joinRoomButton.onClick.AddListener(JoinRoom);
        }
        private void JoinRoom()
        {
            GameObject.Find("RoomManager").GetComponent<RoomManager>().JoinRoomByName(textRoom.text);
        }
    }
}
