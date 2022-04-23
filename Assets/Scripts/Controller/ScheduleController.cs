using System;
using UnityEngine;
using UniRx;

namespace timeinfo
{
    public class ScheduleController : BaseController
    {
        private UITimeView uiTimeView;
        private TimeInfoModel timeInfoModel;
        private bool isIntervalTime;

        protected override void OnInitialize()
        {
            uiTimeView = common.ControllerManager.instance.GetManagedController<TimeInfoController>().getUITimeView();
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
            uiTimeView.OnClickedScheduleButtonAction += OnClickedScheduleButton;
        }

        private void OnDisable()
        {
            uiTimeView.OnClickedScheduleButtonAction -= OnClickedScheduleButton;
        }

        private void Start()
        {
        }

        private void OnClickedScheduleButton()
        {

        }
    }
}