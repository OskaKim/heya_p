using System;
using UnityEngine;
using UniRx;

namespace timeinfo
{
    public class TimeInfoController : BaseController
    {
        [SerializeField] private UITimeView uiTimeView;
        private TimeInfoModel timeInfoModel;

        protected override void SetupModels()
        {
            modelInfoHolder.AddModel(out timeInfoModel);

            Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                timeInfoModel.Tick();
            });
        }

        protected override void SetupViews()
        {
            timeInfoModel.OnUpdateGameTime.Subscribe(currentGameTime =>
            {
                uiTimeView.UpdateTime(currentGameTime);
            });
        }
    }
}