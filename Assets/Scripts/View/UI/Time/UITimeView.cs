using UnityEngine;
using TMPro;
using System;
using System.Globalization;

namespace timeinfo
{
    public class UITimeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text dayText;

        public void UpdateTime(DateTime updatedTime)
        {
            Debug.Log(updatedTime.ToString("MM/dd/yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
            //dayText.text = 
        }
    }
}