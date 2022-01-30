using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using grid;

public class RoomTopPresent : MonoBehaviour
{
    [SerializeField] private UIFurnitureInstallView uiFurnitureInstallView;
    [SerializeField] private UIFurnitureScrollViewView uiFurnitureScrollViewView;
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
    }

    private void ObserveInstallFurniture()
    {
        uiFurnitureScrollViewView.OnSelectFurniture.Subscribe(furnitureId =>
        {
            installFurnitureModel.SelectedFurniture.Value = furnitureId;
        });

        uiFurnitureInstallView.OnInstallFinish = () =>
        {
            installFurnitureModel.SelectedFurniture.Value = -1;
        };

        installFurnitureModel.SelectedFurniture
        .ObserveEveryValueChanged(x => x.Value)
        .Where(x => x >= 0)
        .Subscribe(selectFurnitureId =>
        {
            uiFurnitureInstallView.SelectedFurniture = installFurnitureModel.GetFurnitureTile(selectFurnitureId);
        });
    }
}
