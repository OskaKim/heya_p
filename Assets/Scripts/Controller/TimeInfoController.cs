using System;
using UnityEngine;
using UniRx;

namespace timeinfo
{
    public class TimeInfoController : BaseController
    {
        [SerializeField] private UITimeView uiTimeView;
        private TimeInfoModel timeInfoModel;
        private bool isIntervalTime;
        private void Awake()
        {
            isIntervalTime = Definitions.DefaultInteravalTimeConfig;
        }
        private void OnEnable()
        {
            uiTimeView.OnPlayPauseButtonClicked += OnUpdatePlayMode;
        }
        private void OnDisable()
        {
            uiTimeView.OnPlayPauseButtonClicked -= OnUpdatePlayMode;
        }

        protected override void SetupModels()
        {
            modelInfoHolder.AddModel(out timeInfoModel);

            Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(_ => isIntervalTime)
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

        private void OnUpdatePlayMode(bool isPlaying)
        {
            isIntervalTime = isPlaying;
        }
    }
}