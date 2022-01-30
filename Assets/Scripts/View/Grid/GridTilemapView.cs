using UnityEngine;
using UnityEngine.Tilemaps;

namespace grid
{
    public class GridTilemapView : MonoBehaviour
    {
        [SerializeField] private Tilemap[] tilemaps = new Tilemap[(int)TileMapType.Max];
        public void SetColor(TileMapType type, Vector3Int pos, Color color)
        {
            tilemaps[(int)type].SetColor(pos, color);
        }
        public void SetTile(TileMapType type, Vector3Int pos, TileBase tile)
        {
            tilemaps[(int)type].SetTileFlags(pos, TileFlags.None);
            tilemaps[(int)type].SetTile(pos, tile);
        }
    }
}