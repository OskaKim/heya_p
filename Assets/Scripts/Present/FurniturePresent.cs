using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class FurniturePresent : BasePresent
{
    #region view
    [SerializeField] UIFurnitureStatusView uiFurnitureStatusView;
    #endregion

    #region controller
    #endregion

    #region model
    [SerializeField] FurnitureManagerModel furnitureManagerModel;
    #endregion

    protected override void InitializeControllers()
    {
    }

    protected override void SetupModels()
    {
    }

    protected override void SetupViews()
    {
        furnitureManagerModel.OnClickFurniture += (FurnitureManagerObject furnitureManagerObject) => {
            var pos = furnitureManagerObject.FurnitureManagerGameObject.transform.position;
            uiFurnitureStatusView.Show(pos);
        };
    }
}