using System;
using UnityEngine;
using UniRx;

namespace timeinfo
{
    public class TimeInfoController : BaseController
    {
        private UITimeView uiTimeView;
        private TimeInfoModel timeInfoModel;
        private bool isIntervalTime;

        private void Awake()
        {
            uiTimeView = common.ViewManager.instance.CreateViewObject<UITimeView>();
            modelInfoHolder.AddModel(out timeInfoModel);
        }

        private void OnEnable()
        {
            uiTimeView.OnPlayPauseButtonClicked += OnUpdatePlayMode;
        }

        private void OnDisable()
        {
            uiTimeView.OnPlayPauseButtonClicked -= OnUpdatePlayMode;
        }

        private void Start()
        {
            isIntervalTime = Definitions.DefaultInteravalTimeConfig;

            Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(_ => isIntervalTime)
            .Subscribe(_ =>
            {
                timeInfoModel.Tick();
            });

            timeInfoModel.OnUpdateGameTime.Subscribe(currentGameTime =>
            {
                uiTimeView.UpdateTime(currentGameTime);
            });

            // 1번 Tick을 진행해두지 않으면 ui가 갱신되지 않기 때문에 미리 업데이트
            timeInfoModel.Tick();
        }

        private void OnUpdatePlayMode(bool isPlaying)
        {
            isIntervalTime = isPlaying;
        }
    }
}