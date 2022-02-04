using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class RoomTopPresent : BasePresent
{
    // todo : 인스펙터를 통해 참조할 객체들이 계속 늘어날 수도 있으니 타입별로 Holder를 준비해서 그 Holder를 참조하는 식으로 받기
    #region view
    [SerializeField] private UIFurnitureInstallView uiFurnitureInstallView;
    [SerializeField] private UIFurnitureScrollViewView uiFurnitureScrollViewView;
    [SerializeField] private GridCharacterView gridCharacterView;
    [SerializeField] private GridTilemapView gridTilemapView;
    [SerializeField] private UITimeView uiTimeView;
    #endregion

    #region controller
    private GridInstallController gridInstallController;
    private TimeInfoController timeInfoController;
    #endregion

    #region model
    private InstallFurnitureModel installFurnitureModel;
    private TimeInfoModel timeInfoModel;
    #endregion

    protected override void InitializeModels()
    {
        installFurnitureModel = new InstallFurnitureModel();
        timeInfoModel = new TimeInfoModel();
    }
    protected override void InitializeControllers()
    {
        gridInstallController = new GridInstallController(installFurnitureModel, uiFurnitureInstallView, gridTilemapView);
        timeInfoController = new TimeInfoController(timeInfoModel, uiTimeView);
    }
    protected override void SetupModels()
    {
        SetupTimeInfoModel();
    }
    protected override void SetupViews()
    {
        gridInstallController.Setup();
        gridCharacterStartView();
        UiFurnitureScrollViewStartView();
    }

    #region view
    private void gridCharacterStartView()
    {
        gridCharacterView.StartView();
    }
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

    #region controller
    #endregion

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
