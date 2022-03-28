using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.Tilemaps;

namespace grid
{
    public class GridInstallController : BaseController
    {
        private GridTilemapView gridTilemapView;
        
        private FurniturePreviewDrawService furniturePreviewDrawService;

        private FurnitureManagerModel furnitureManagerModel;
        private InstallFurnitureModel installFurnitureModel;

        // todo : 모델의 정보를 사용하도록 하기
        private Vector3Int installPosCache;
        private List<Vector3Int> installRestrictAreasCache = new List<Vector3Int>();

        protected override void Start()
        {
            gridTilemapView = common.ViewManager.instance.CreateViewObject<GridTilemapView>();
            furniturePreviewDrawService = new FurniturePreviewDrawService();

            modelInfoHolder.AddModel(out furnitureManagerModel);
            modelInfoHolder.AddModel(out installFurnitureModel);

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
                foreach (var installRange in installRestrictAreasCache)
                {
                    gridTilemapView.SetColor(grid.TileMapType.Floor, installRange, Color.white);
                }
                
                gridTilemapView.SetTile(grid.TileMapType.FurniturePreview, installPosCache, null);

                // note : 프리뷰 타일 그리기
                var selectedFurniture = installFurnitureModel.GetSelectedFurnitureTile();
                var selectedFurnitureInstallRestrictedAreas = installFurnitureModel.InstallRestrictedAreas;
                furniturePreviewDrawService.DrawPreview(pos, selectedFurnitureInstallRestrictedAreas, selectedFurniture);

                // note : 나중에 지우기 위해 캐시
                installPosCache = pos;
                installRestrictAreasCache = new List<Vector3Int>(selectedFurnitureInstallRestrictedAreas);
            });

            // note : 선택된 타일이 있고, 입력이 있으면 설치
            Observable.EveryUpdate()
            .Where(_ =>
            {
                var checkInput = Input.GetMouseButtonDown(0);
                var isExistSelectedFurniture = installFurnitureModel.ExistSelectedFurnitureTile();

                // note : 이미 설치된 타일이 타일 설치 영역에 있음
                if (furnitureManagerModel.IsInInstallRestrictArea(installRestrictAreasCache)) return false;
                
                return checkInput && isExistSelectedFurniture;
            })
            .Subscribe(_ =>
            {
                // note : 이미 표시되고 있는 프리뷰 타일을 지우기
                foreach (var installRestrictArea in installRestrictAreasCache)
                {
                    gridTilemapView.SetColor(grid.TileMapType.Floor, installRestrictArea, Color.white);
                }
                gridTilemapView.SetTile(grid.TileMapType.FurniturePreview, installPosCache, null);

                // note : 타일 설치
                var selectedFurniture = installFurnitureModel.GetSelectedFurnitureTile();
                var installPos = installFurnitureModel.InstallPos;
                gridTilemapView.SetTile(grid.TileMapType.Furniture, installPos.Value, selectedFurniture);
                var installedTile = gridTilemapView.GetTile(grid.TileMapType.Furniture, installPos.Value);
                AttachSpriteObjectObject(installFurnitureModel.SelectedFurniture.Value, installPos.Value, installRestrictAreasCache);
                installFurnitureModel.InstallFurniture();
            });

            furniturePreviewDrawService.OnChangeTilemapColor = (TileMapType type, Vector3Int pos, Color color) =>
            {
                gridTilemapView.SetColor(type, pos, color);
            };

            furniturePreviewDrawService.OnChangeTiemapTile = (TileMapType type, Vector3Int pos, TileBase tile) =>
            {
                gridTilemapView.SetTile(type, pos, tile);
            };

            furniturePreviewDrawService.IsTileExistFurnitureAlready = (Vector3Int pos) =>
            {
                // note : 이미 설치된 타일이 타일 설치 영역에 있음
                return furnitureManagerModel.IsInInstallRestrictArea(pos);
            };

            furnitureManagerModel.Setup();
            furnitureManagerModel.OnRotateFurniture += (Vector3Int pos, FurnitureDirectionType furnitureDirection) =>
            {
                gridTilemapView.RotateTile(grid.TileMapType.Furniture, pos, furnitureDirection);
                gridTilemapView.RotateTile(grid.TileMapType.Decorate, pos, furnitureDirection);
            };
        }

        // 생성한 가구 타일의 위치에 관리용 오브젝트를 생성. 가구 클릭 판정등에 사용
        private void AttachSpriteObjectObject(int furnitureId, Vector3Int installPos, List<Vector3Int> installRange)
        {
            var tileWorldPos = gridTilemapView.GetTileWorldPos(grid.TileMapType.Furniture, installPos, false);
            var furnitureObject = new GameObject();
            furnitureObject.transform.position = tileWorldPos;
            var spriteRenderer = furnitureObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = installFurnitureModel.GetFurnitureSprite(furnitureId);
            spriteRenderer.enabled = false;

            // NOTE : 유니티 엔진의 문제인지 모르겠으나, 이하처럼 콜라이더를 껏다가 다음 프레임에 활성화 하도록 해야 클릭 처리가 가능.
            var collider = furnitureObject.AddComponent<PolygonCollider2D>();
            collider.enabled = false;
            Observable.NextFrame().Subscribe(_ =>
            {
                collider.enabled = true;
            });

            var furnitureManagerObject = furnitureManagerModel.AddfurnitureManagerObjects(furnitureId, furnitureObject, FurnitureDirectionType.Left, installPos);
            furnitureObject.name = $"{furnitureManagerObject.Serial}";
        }
    }
}
