using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Tilemaps;

namespace grid
{

    public class GridInstallController
    {
        private InstallFurnitureModel installFurnitureModel;
        private UIFurnitureInstallView uiFurnitureInstallView;
        private GridTilemapView gridTilemapView;
        public GridInstallController(InstallFurnitureModel installFurnitureModel,
        UIFurnitureInstallView uiFurnitureInstallView,
        GridTilemapView gridTilemapView)
        {
            this.installFurnitureModel = installFurnitureModel;
            this.uiFurnitureInstallView = uiFurnitureInstallView;
            this.gridTilemapView = gridTilemapView;
        }

        // todo : 내용이 복잡해졌기 때문에 GridInstallController같은 클래스에 모델의 인터페이스를 받아서 처리하도록 하기
        // todo : 모델의 정보를 사용하도록 하기
        private Vector3Int installPosCache;
        private List<Vector3Int> installRangeCache = new List<Vector3Int>();

        public void Setup()
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
                // note : 이미 표시되고 있는 프리뷰 타일을 지우기
                foreach (var installRange in installRangeCache)
                {
                    gridTilemapView.SetColor(grid.TileMapType.Floor, installRange, Color.white);
                }
                gridTilemapView.SetTile(grid.TileMapType.FurniturePreview, installPosCache, null);

                // note : 프리뷰 타일 그리기
                var selectedFurniture = installFurnitureModel.GetSelectedFurnitureTile();
                var selectedFurnitureRange = installFurnitureModel.InstallRange.Value;
                uiFurnitureInstallView.DrawPreview(pos, selectedFurnitureRange, selectedFurniture);

                // note : 나중에 지우기 위해 캐시
                installPosCache = pos;
                installRangeCache = new List<Vector3Int>(selectedFurnitureRange);
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
                // note : 이미 표시되고 있는 프리뷰 타일을 지우기
                foreach (var installRange in installRangeCache)
                {
                    gridTilemapView.SetColor(grid.TileMapType.Floor, installRange, Color.white);
                }
                gridTilemapView.SetTile(grid.TileMapType.FurniturePreview, installPosCache, null);

                // note : 타일 설치
                var selectedFurniture = installFurnitureModel.GetSelectedFurnitureTile();
                var installPos = installFurnitureModel.InstallPos;
                gridTilemapView.SetTile(grid.TileMapType.Furniture, installPos.Value, selectedFurniture);
                AttachSpriteObjectObject(installPos.Value);
                installFurnitureModel.InstallFurniture();
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

        // 생성한 가구 타일의 위치에 관리용 오브젝트를 생성. 가구 클릭 판정등에 사용
        private void AttachSpriteObjectObject(Vector3Int installPos)
        {
            var tileWorldPos = gridTilemapView.GetTileWorldPos(grid.TileMapType.Furniture, installPos);
            var furnitureColliderObject = new GameObject($"furniture collider {installFurnitureModel.SelectedFurniture.Value}");
            furnitureColliderObject.transform.position = tileWorldPos;
            var spriteRenderer = furnitureColliderObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite =  installFurnitureModel.GetFurnitureSprite(installFurnitureModel.SelectedFurniture.Value);
            spriteRenderer.enabled = false;
            var collider = furnitureColliderObject.AddComponent<PolygonCollider2D>();

            // NOTE : 타일상의 위치와 차이가 있기 때문에 보정
            collider.offset = new Vector2(0, 0.25f);
        }
    }
}
