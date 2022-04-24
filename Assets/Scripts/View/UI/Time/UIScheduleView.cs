using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace timeinfo
{
    public class UIScheduleView : MonoBehaviour
    {
        // note : 스케줄은 현재 시간으로 부터 최대 몇 시간까지 표시할 것인가
        const int MaxScheduleTimeLength = 24;

        [SerializeField] private UIScheduleBaseCellContent baseCellPrefab;
        [SerializeField] private Transform scrollViewContentTop;

        private UIScheduleBaseCellContent[] baseCells;

        private void Awake()
        {
            var roomUI = GameObject.Find("RoomUI");
            transform.SetParent(roomUI.transform);

            baseCells = new UIScheduleBaseCellContent[MaxScheduleTimeLength];
            for(int i = 0; i < baseCells.Length; ++i)
            {
                baseCells[i] = GameObject.Instantiate<UIScheduleBaseCellContent>(baseCellPrefab, scrollViewContentTop);
            }
        }

        public void OpenScheduleWindow(int currentHour)
        {
            for (int i = 0; i < baseCells.Length; ++i)
            {
                int cellTime = (currentHour + i) % 24; // 하루는 24시간이므로 그걸 넘으면 다음날 취급
                baseCells[i].setTime(cellTime);
            }
        }
    }
}
