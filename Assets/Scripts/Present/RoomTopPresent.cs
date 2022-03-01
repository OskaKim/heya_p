using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class RoomTopPresent : BasePresent
{
    #region view
    [SerializeField] private UIFurnitureInstallView uiFurnitureInstallView;
    [SerializeField] private UIFurnitureScrollViewView uiFurnitureScrollViewView;
    [SerializeField] private GridTilemapView gridTilemapView;
    #endregion

    #region controller
    private GridInstallController gridInstallController;
    #endregion

    #region model
    private InstallFurnitureModel installFurnitureModel;
    private FurnitureManagerModel furnitureManagerModel;
    #endregion

    protected override void InitializeControllers()
    {
        gridInstallController = new GridInstallController(furnitureManagerModel, installFurnitureModel, uiFurnitureInstallView, gridTilemapView);
    }
    protected override void SetupModels()
    {
        installFurnitureModel = InstallFurnitureModel.instance;
        furnitureManagerModel = FurnitureManagerModel.instance;
    }
    protected override void SetupViews()
    {
        gridInstallController.Setup();
        UiFurnitureScrollViewStartView();
    }

    #region view
    private void UiFurnitureScrollViewStartView()
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
    #endregion
}
