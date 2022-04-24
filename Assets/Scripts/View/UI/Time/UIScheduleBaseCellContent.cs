using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace timeinfo
{
    // note : content는 view의 하위 개념
    // 컨트롤러에서 직접 제어하는게 부적절할 경우(ui의 셀 등)
    public class UIScheduleBaseCellContent : MonoBehaviour
    {
        [SerializeField] private Text timeText;

        public void setTime(int hour)
        {
            timeText.text = $"{hour.ToString("00")}:00";
        }
    }
}