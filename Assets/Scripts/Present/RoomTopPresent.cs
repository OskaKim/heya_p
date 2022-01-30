using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class RoomTopPresent : MonoBehaviour
{
    // TODO : 외부 데이터나 세이브로부터 로드 되도록 하기
    [SerializeField] private Vector3Int characterInitializePosition;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private UIInstallFurnitureView uiInstallFurnitureView;
    [SerializeField] private UIFurnitureScrollView uiFurnitureScrollView;

    private InstallFurnitureModel installFurnitureModel = new InstallFurnitureModel();

    private void Start()
    {
        var worldPos = grid.CellToWorld(characterInitializePosition);
        GameObject.Instantiate(characterPrefab, worldPos, Quaternion.identity);

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
