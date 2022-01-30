using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using grid;
using UnityEngine.Tilemaps;

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
        FurnitureInstallViewStartView();
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
    private void FurnitureInstallViewStartView()
    {
        var targetType = installFurnitureModel.GetInstallTilemapType();
        gridTilemapView.ObserveOnStayTilemap(targetType, uiFurnitureInstallView.ClearAndDraw);

        uiFurnitureInstallView.OnInstallFinish = () =>
        {
            installFurnitureModel.SelectedFurniture.Value = -1;
        };

        uiFurnitureInstallView.OnChangeTilemapColor = (TileMapType type, Vector3Int pos, Color color) =>
        {
            gridTilemapView.SetColor(type, pos, color);
        };

        uiFurnitureInstallView.OnChangeTiemapTile = (TileMapType type, Vector3Int pos, TileBase tile) =>
        {
            gridTilemapView.SetTile(type, pos, tile);
        };

        uiFurnitureInstallView.GetTilemapTile = (TileMapType type, Vector3Int pos) =>
        {
            return gridTilemapView.GetTile(type, pos);
        };

        uiFurnitureInstallView.OnInstallTile = () =>
        {
            var isExistSelectedFurniture = installFurnitureModel.GetFurnitureTile(installFurnitureModel.SelectedFurniture.Value);
            return isExistSelectedFurniture && Input.GetMouseButtonDown(0);
        };

        // view의 selectedFurniture를 model과 일치시키기
        // todo : view가 상태를 가지지 않도록 수정
        installFurnitureModel.SelectedFurniture
        .ObserveEveryValueChanged(x => x.Value)
        .Where(x => x >= 0)
        .Subscribe(selectFurnitureId =>
        {
            uiFurnitureInstallView.SelectedFurniture = installFurnitureModel.GetFurnitureTile(selectFurnitureId);
        });
    }
}
