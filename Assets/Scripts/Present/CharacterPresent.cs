using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class CharacterPresent : BasePresent
{
    #region view
    #endregion

    #region controller
    #endregion

    #region model
    [SerializeField]
    private CharacterAIModel characterAIModel;
    [SerializeField]
    private TimeInfoModel timeInfoModel;
    #endregion

    protected override void InitializeControllers()
    {
        throw new NotImplementedException();
    }

    protected override void InitializeModels()
    {
        throw new NotImplementedException();
    }

    protected override void SetupModels()
    {
        throw new NotImplementedException();
    }

    protected override void SetupViews()
    {
        throw new NotImplementedException();
    }
}