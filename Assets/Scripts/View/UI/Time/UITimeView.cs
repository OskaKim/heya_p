using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;

namespace timeinfo
{
    public class UITimeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private Button playPauseButton;
        [SerializeField] private Text playPauseButtonText;

        public event Action<bool> OnPlayPauseButtonClicked;
        private bool isPlaying;

        private void Awake()
        {
            isPlaying = Definitions.DefaultInteravalTimeConfig;
        }

        public void UpdateTime(DateTime updatedTime)
        {
            //Debug.Log(updatedTime.ToString("MM/dd/yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
            dayText.text = updatedTime.ToString("MM/dd", DateTimeFormatInfo.InvariantInfo);
            timeText.text = updatedTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);
        }

        public void OnClickedButton()
        {
            isPlaying = !isPlaying;
            playPauseButtonText.text = isPlaying ? "Play" : "Pause";
            OnPlayPauseButtonClicked?.Invoke(isPlaying);
        }
    }
}