using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class TimeInfoPresent : BasePresent
{
    #region view
    [SerializeField] private UITimeView uiTimeView;
    #endregion

    #region controller
    private TimeInfoController timeInfoController;
    #endregion

    #region model
    [SerializeField]
    private TimeInfoModel timeInfoModel;
    #endregion

    protected override void InitializeControllers()
    {
        timeInfoController = new TimeInfoController(timeInfoModel, uiTimeView);
    }
    protected override void SetupModels()
    {
        SetupTimeInfoModel();
    }
    protected override void SetupViews()
    {
        
    }

    #region model

    private void SetupTimeInfoModel()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
        .Subscribe(_ =>
        {
            timeInfoModel.Tick();
        });
    }
    #endregion
}
