using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace timeinfo
{
    public class TimeInfoController
    {
        private TimeInfoModel timeInfoModel;
        private DateTime startDateTime;
        private double startTime;
        private UITimeView uiTimeView;

        public TimeInfoController(TimeInfoModel timeInfoModel, UITimeView uiTimeView)
        {
            this.timeInfoModel = timeInfoModel;
            this.uiTimeView = uiTimeView;

            timeInfoModel.OnUpdateGameTime.Subscribe(currentGameTime =>
            {
                uiTimeView.UpdateTime(currentGameTime);
            });
        }
    }
}