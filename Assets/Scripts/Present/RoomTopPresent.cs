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
        ObserveInstallFurniture();
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
            installFurnitureModel.FurnitureInstallModeToggle.Value = false; 
        };
        
        installFurnitureModel.SelectedFurniture
        .ObserveEveryValueChanged(x => x.Value)
        .Where(x => x >= 0)
        .Subscribe(selectFurnitureId =>
        {
            // todo : UIInstallFurnitureView의 선택 타일 갱신
            installFurnitureModel.FurnitureInstallModeToggle.Value = true;
        });

        installFurnitureModel.FurnitureInstallModeToggle
        .ObserveEveryValueChanged(x => x.Value)
        .Subscribe(installMode =>
        {
            // todo : UIInstallFurnitureView의 선택 상태 갱신
            uiInstallFurnitureView.IsInstallMode = installMode;
            Debug.Log($"FurnitureInstallModeToggle{installMode}");
        });


    }
}
