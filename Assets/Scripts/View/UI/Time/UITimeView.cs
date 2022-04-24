using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

namespace timeinfo
{
    public class UITimeView : MonoBehaviour
    {
        [SerializeField] private Text dayText;
        [SerializeField] private Text timeText;
        [SerializeField] private Button playPauseButton;
        [SerializeField] private Image playPauseButtonImage;
        [SerializeField] private Sprite playPauseButtonStartImageResource;
        [SerializeField] private Sprite playPauseButtonPauseImageResource;

        public event Action<bool> OnPlayPauseButtonClicked;
        public event Action OnClickedScheduleButtonAction;
        private bool isPlaying;

        private void Awake()
        {
            var roomUI = GameObject.Find("RoomUI");
            transform.SetParent(roomUI.transform);

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
            playPauseButtonImage.sprite = isPlaying ? playPauseButtonStartImageResource : playPauseButtonPauseImageResource;
            OnPlayPauseButtonClicked?.Invoke(isPlaying);
        }

        public void OnClickedScheduleButton()
        {
            OnClickedScheduleButtonAction?.Invoke();
        }
    }
}