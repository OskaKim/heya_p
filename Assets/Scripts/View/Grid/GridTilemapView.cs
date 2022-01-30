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
        public void ObserveOnStayTilemap(TileMapType type, Action<Vector3Int> onStayTilemap)
        {
            var tilemapTouchHandler = Utility.InputUtility.GetTilemapTouchHandler(GetTilemap(type));
            tilemapTouchHandler.OnStayTilemap.Subscribe(pos => onStayTilemap(pos));
        }
    }
}