using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace timeinfo
{
    public class UIScheduleView : MonoBehaviour
    {
        private void Awake() {
            var roomUI = GameObject.Find("RoomUI");
            transform.SetParent(roomUI.transform);
        }
    }
}
