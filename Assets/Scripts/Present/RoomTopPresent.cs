using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class RoomTopPresent : MonoBehaviour
{
    [SerializeField] private UIInstallFurnitureView uiInstallFurnitureView;
    [SerializeField] private UIFurnitureScrollView uiFurnitureScrollView;

    private InstallFurnitureModel installFurnitureModel = new InstallFurnitureModel();

    private void Start()
    {
        UiFurnitureScrollViewStartView();
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

        uiFurnitureScrollView.OnSelectFurniture.Subscribe(furnitureId =>
        {
            installFurnitureModel.SelectedFurniture.Value = furnitureId;
            Debug.Log($"selected id : {installFurnitureModel.SelectedFurniture}");
        });

        uiFurnitureScrollView.StartView(param);
    }
}
