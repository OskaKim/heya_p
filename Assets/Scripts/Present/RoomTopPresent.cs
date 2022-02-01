using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using grid;
using UnityEngine.Tilemaps;

public class RoomTopPresent : MonoBehaviour
{
    // todo : 인스펙터를 통해 참조할 객체들이 계속 늘어날 수도 있으니 타입별로 Holder를 준비해서 그 Holder를 참조하는 식으로 받기
    [SerializeField] private UIFurnitureInstallView uiFurnitureInstallView;
    [SerializeField] private UIFurnitureScrollViewView uiFurnitureScrollViewView;
    [SerializeField] private GridCharacterView gridCharacterView;
    [SerializeField] private GridTilemapView gridTilemapView;

    private InstallFurnitureModel installFurnitureModel = new InstallFurnitureModel();
    private GridInstallController gridInstallController;
    private void Awake()
    {
        gridInstallController = new GridInstallController(installFurnitureModel, uiFurnitureInstallView, gridTilemapView);
    }
    private void Start()
    {
        gridCharacterStartView();
        UiFurnitureScrollViewStartView();
        gridInstallController.FurnitureInstallViewStartView();
    }

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
}
