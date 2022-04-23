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

        public UITimeView getUITimeView()
        {
            return uiTimeView;
        }

        protected override void OnInitialize()
        {
            uiTimeView = common.ViewManager.instance.CreateViewObject<UITimeView>();
            modelInfoHolder.AddModel(out timeInfoModel);
        }

        protected override void OnFinalize()
        {
            // todo : view의 삭제(예약)
            // HogeView.FinalizeView();

            // todo : model의 삭제(참조 카운트 -1)
            // modelInfoHolder.RemoveModel(hogeModel)
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