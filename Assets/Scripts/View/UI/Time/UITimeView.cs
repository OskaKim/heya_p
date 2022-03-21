using UnityEngine;
using TMPro;
using System;
using System.Globalization;

namespace timeinfo
{
    public class UITimeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text timeText;

        public void UpdateTime(DateTime updatedTime)
        {
            //Debug.Log(updatedTime.ToString("MM/dd/yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
            dayText.text = updatedTime.ToString("MM/dd", DateTimeFormatInfo.InvariantInfo);
            timeText.text = updatedTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);
        }
    }
}