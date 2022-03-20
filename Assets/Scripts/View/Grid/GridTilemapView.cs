using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UniRx;

namespace grid
{
    public class GridTilemapView : MonoBehaviour
    {
        [SerializeField] private Tilemap[] tilemaps = new Tilemap[(int)TileMapType.Max];
        private Tilemap GetTilemap(TileMapType type)
        {
            return tilemaps[(int)type];
        }

        // grid위치에서 worldpos를 취득.
        public Vector3 GetTileWorldPos(TileMapType type, Vector3Int pos, bool isCorrectRenderPrioirty)
        {
            var worldPos = GetTilemap(type).CellToWorld(pos);

            // NOTE : CellToWorld로 취득되는 위치가 실제 위치와 어째서인지 차이가 있으므로 offset
            var offset = new Vector2(0, 0.25f);
            worldPos = new Vector3(worldPos.x + offset.x, worldPos.y + offset.y, worldPos.z);

            if (!isCorrectRenderPrioirty) return worldPos;

            return GridUtility.GetCorrectGridWorldPosition(worldPos);
        }
        public Color GetColor(TileMapType type, Vector3Int pos)
        {
            return GetTilemap(type).GetColor(pos);
        }
        public void SetColor(TileMapType type, Vector3Int pos, Color color)
        {
            GetTilemap(type).SetTileFlags(pos, TileFlags.None);
            GetTilemap(type).SetColor(pos, color);
        }
        public TileBase GetTile(TileMapType type, Vector3Int pos)
        {
            return GetTilemap(type).GetTile(pos);
        }
        public void SetTile(TileMapType type, Vector3Int pos, TileBase tile)
        {
            GetTilemap(type).SetTileFlags(pos, TileFlags.None);
            GetTilemap(type).SetTile(pos, tile);
        }
        public void RotateTile(TileMapType type, Vector3Int pos, FurnitureDirectionType furnitureDirection)
        {
            // tilemap의 매트릭스에 대한 래퍼 함수가 없어서 매트릭스를 직접 수정해야 함
            var matrix = GetTilemap(type).GetTransformMatrix(pos);
            var originPosition = matrix.GetColumn(3);
            var originScale = new Vector3(
                matrix.GetColumn(0).magnitude,
                matrix.GetColumn(1).magnitude,
                matrix.GetColumn(2).magnitude
            );
            var originEuler = Quaternion.LookRotation(
                matrix.GetColumn(2),
                matrix.GetColumn(1)
            );

            var originDirection = originEuler == Quaternion.Euler(0, 0, 0) ? FurnitureDirectionType.Left : FurnitureDirectionType.Right;
            
            var euler =
                furnitureDirection == FurnitureDirectionType.Left ? Quaternion.Euler(0, 0, 0)
                : Quaternion.Euler(0, -180, 0);

            bool isDirectionUpdated = originDirection != furnitureDirection;
            var offset =
                isDirectionUpdated ? new Vector4(originPosition.x * -1, originPosition.y, originPosition.z)
                : originPosition;

            matrix = Matrix4x4.TRS(offset, euler, originScale);
            GetTilemap(type).SetTransformMatrix(pos, matrix);
        }
        public void OffsetTile(TileMapType type, Vector3Int pos, Vector3 offset)
        {
            // tilemap의 매트릭스에 대한 래퍼 함수가 없어서 매트릭스를 직접 수정해야 함
            var matrix = GetTilemap(type).GetTransformMatrix(pos);

            var originEuler = Quaternion.LookRotation(
                matrix.GetColumn(2),
                matrix.GetColumn(1)
            );

            var originScale = new Vector3(
                matrix.GetColumn(0).magnitude,
                matrix.GetColumn(1).magnitude,
                matrix.GetColumn(2).magnitude
            );

            matrix = Matrix4x4.TRS(offset, originEuler, originScale);
            GetTilemap(type).SetTransformMatrix(pos, matrix);
        }
        public void ObserveOnStayTilemap(TileMapType type, Action<Vector3Int> action)
        {
            var tilemapTouchHandler = Utility.InputUtility.GetTilemapTouchHandler(GetTilemap(type));
            tilemapTouchHandler.OnStayTilemap.Subscribe(pos => action(pos));
        }
        public void ObserveOnTouchDownTilemap(TileMapType type, Action<Vector3Int> action)
        {
            var tilemapTouchHandler = Utility.InputUtility.GetTilemapTouchHandler(GetTilemap(type));
            tilemapTouchHandler.OnTouchDownTilemap.Subscribe(pos => action(pos));
        }
        public void ObserveOnTouchPressTilemap(TileMapType type, Action<Vector3Int> action)
        {
            var tilemapTouchHandler = Utility.InputUtility.GetTilemapTouchHandler(GetTilemap(type));
            tilemapTouchHandler.OnTouchPressTilemap.Subscribe(pos => action(pos));
        }
        public void ObserveOnTouchUpTilemap(TileMapType type, Action<Vector3Int> action)
        {
            var tilemapTouchHandler = Utility.InputUtility.GetTilemapTouchHandler(GetTilemap(type));
            tilemapTouchHandler.OnTouchUpTilemap.Subscribe(pos => action(pos));
        }
    }
}