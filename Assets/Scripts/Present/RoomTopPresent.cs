using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using grid;

public class RoomTopPresent : MonoBehaviour
{
    [SerializeField] private UIInstallFurnitureView uiInstallFurnitureView;
    [SerializeField] private UIFurnitureScrollView uiFurnitureScrollView;
    [SerializeField] private GridCharacterView gridCharacterView;
    [SerializeField] private GridTilemapView gridTilemapView;

    private InstallFurnitureModel installFurnitureModel = new InstallFurnitureModel();

    private void Start()
    {
        gridCharacterStartView();
        UiFurnitureScrollViewStartView();
        ObserveInstallFurniture();
    }

    private void gridCharacterStartView()
    {
        gridCharacterView.StartView();
    }

    private void UiFurnitureScrollViewStartView()
    {
        UIFurnitureScrollView.Param param;

        param.furnitureScrollDataList = new List<UIFurnitureScrollView.Param.FurnitureScrollData>();

        foreach (var originData in installFurnitureModel.FurnitureDataBase)
        {
            UIFurnitureScrollView.Param.FurnitureScrollData uiScrollData;
            uiScrollData.sprite = originData.sprite;
            uiScrollData.id = originData.id;
            param.furnitureScrollDataList.Add(uiScrollData);
        }

        uiFurnitureScrollView.StartView(param);
    }

    private void ObserveInstallFurniture()
    {
        uiFurnitureScrollView.OnSelectFurniture.Subscribe(furnitureId =>
        {
            installFurnitureModel.SelectedFurniture.Value = furnitureId;
        });

        uiInstallFurnitureView.OnInstallFinish = () =>
        {
            installFurnitureModel.SelectedFurniture.Value = -1;
        };

        installFurnitureModel.SelectedFurniture
        .ObserveEveryValueChanged(x => x.Value)
        .Where(x => x >= 0)
        .Subscribe(selectFurnitureId =>
        {
            uiInstallFurnitureView.SelectedFurniture = installFurnitureModel.GetFurnitureTile(selectFurnitureId);
        });
    }
}
