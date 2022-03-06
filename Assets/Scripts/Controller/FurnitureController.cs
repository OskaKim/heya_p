using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class FurnitureController : BaseController
{
    #region view
    [SerializeField] UIFurnitureStatusView uiFurnitureStatusView;
    #endregion

    #region controller
    #endregion

    #region model
    private FurnitureManagerModel furnitureManagerModel;
    private FurnitureDecorateModel furnitureDecorateModel;
    #endregion

    private int? selectFurniture;

    protected override void SetupModels()
    {
        modelInfoHolder.AddModel(out furnitureManagerModel);
        modelInfoHolder.AddModel(out furnitureDecorateModel);
    }

    protected override void SetupViews()
    {
        furnitureManagerModel.OnClickFurniture += (FurnitureManagerObject furnitureManagerObject) => {
            selectFurniture = furnitureManagerObject.Id;
            var pos = furnitureManagerObject.FurnitureManagerGameObject.transform.position;
            uiFurnitureStatusView.Show(pos);
        };
        uiFurnitureStatusView.OnClickRotateButton += () => {
            furnitureManagerModel.ReverseFurnitureDirection(selectFurniture.Value);
        };
        uiFurnitureStatusView.OnClickDecorateButton += () => {
            furnitureDecorateModel.DecorateFurniture(selectFurniture.Value);
        };
    }
}