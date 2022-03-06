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
        public Vector3 GetTileWorldPos(TileMapType type, Vector3Int pos)
        {
            return GetTilemap(type).CellToWorld(pos);
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
            var euler =
             furnitureDirection == FurnitureDirectionType.Left ? Quaternion.Euler(0, 0, 0)
             : Quaternion.Euler(0, -180, 0);

            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, euler, Vector3.one);
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