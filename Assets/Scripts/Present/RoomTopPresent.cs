using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using grid;
using timeinfo;
using UnityEngine.Tilemaps;
using System;

public class RoomTopPresent : MonoBehaviour
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

    private void Awake() {
        Debug.Log("RoomTopPresent");   
        installFurnitureModel = new InstallFurnitureModel();
        timeInfoModel = new TimeInfoModel(); 
    }
    // note : 객체 초기화 순서
    // database(static) -> 해당 씬의 present -> present 내의 model, controller, view 순서
    private void Start()
    {
        SetupTimeInfoModel();
        InitializeController();
        gridCharacterStartView();
        UiFurnitureScrollViewStartView();
        gridInstallController.FurnitureInstallViewStartView();
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
    private void InitializeController()
    {
        gridInstallController = new GridInstallController(installFurnitureModel, uiFurnitureInstallView, gridTilemapView);
        timeInfoController = new TimeInfoController(timeInfoModel, uiTimeView);
    }
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
