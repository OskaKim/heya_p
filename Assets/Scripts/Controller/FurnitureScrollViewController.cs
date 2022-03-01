using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class FurnitureScrollViewController : BaseController
{
    #region view
    [SerializeField] private UIFurnitureScrollViewView uiFurnitureScrollViewView;
    #endregion

    #region model
    private InstallFurnitureModel installFurnitureModel;
    private FurnitureManagerModel furnitureManagerModel;
    #endregion

    protected override void SetupModels()
    {
        installFurnitureModel = InstallFurnitureModel.instance;
        furnitureManagerModel = FurnitureManagerModel.instance;
    }
    protected override void SetupViews()
    {
        UIFurnitureScrollViewView.Param param;

        param.furnitureScrollDataList = new List<UIFurnitureScrollViewView.Param.FurnitureScrollData>();

        foreach (var originData in installFurnitureModel.FurnitureDataBase)
        {
            UIFurnitureScrollViewView.Param.FurnitureScrollData uiScrollData;
            uiScrollData.sprite = originData.sprite;
            uiScrollData.id = originData.id;
            param.furnitureScrollDataList.Add(uiScrollData);
        }

        uiFurnitureScrollViewView.StartView(param);

        uiFurnitureScrollViewView.OnSelectFurniture.Subscribe(furnitureId =>
        {
            installFurnitureModel.SelectedFurniture.Value = furnitureId;
        });
    }
}
