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

    // todo : 내용이 복잡해졌기 때문에 GridInstallController같은 클래스에 모델의 인터페이스를 받아서 처리하도록 하기
    private void FurnitureInstallViewStartView()
    {
        var installTileType = installFurnitureModel.GetInstallTilemapType();

        // note : 타일 입력으로부터 model 타일 위치 갱신
        gridTilemapView.ObserveOnStayTilemap(installTileType, (pos) =>
        {
            var selectedFurniture = installFurnitureModel.GetSelectedFurnitureTile();
            installFurnitureModel.UpdateInstallPos(pos);
        });

        // note : model 타일 위치 갱신시 ui 업데이트
        installFurnitureModel.InstallPos
        .ObserveEveryValueChanged(x => x.Value)
        .Subscribe(pos =>
        {
            var selectedFurniture = installFurnitureModel.GetSelectedFurnitureTile();
            var selectedFurnitureRange = installFurnitureModel.InstallRange.Value;
            uiFurnitureInstallView.DrawPreview(pos, selectedFurnitureRange, selectedFurniture);
        });

        // note : 선택된 타일이 있고, 입력이 있으면 설치
        Observable.EveryUpdate()
        .Where(_ =>
        {
            var checkInput = Input.GetMouseButtonDown(0);
            var isExistSelectedFurniture = installFurnitureModel.ExistSelectedFurnitureTile();
            return checkInput && isExistSelectedFurniture;
        })
        .Subscribe(_ =>
        {
            var selectedFurniture = installFurnitureModel.GetSelectedFurnitureTile();
            var installPos = installFurnitureModel.InstallPos;
            var selectedFurnitureRange = installFurnitureModel.InstallRange.Value;
            uiFurnitureInstallView.ClearInstallPreview();
            gridTilemapView.SetTile(grid.TileMapType.Furniture, installPos.Value, selectedFurniture);
            installFurnitureModel.UnSelectFurniture();
        });

        uiFurnitureInstallView.OnChangeTilemapColor = (TileMapType type, Vector3Int pos, Color color) =>
        {
            gridTilemapView.SetColor(type, pos, color);
        };

        uiFurnitureInstallView.OnChangeTiemapTile = (TileMapType type, Vector3Int pos, TileBase tile) =>
        {
            gridTilemapView.SetTile(type, pos, tile);
        };

        uiFurnitureInstallView.IsTileExistFurnitureAlready = (Vector3Int pos) =>
        {
            return gridTilemapView.GetTile(grid.TileMapType.Furniture, pos);
        };
    }
}
