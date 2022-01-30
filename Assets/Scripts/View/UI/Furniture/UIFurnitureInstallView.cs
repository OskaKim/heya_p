using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class UIFurnitureInstallView : MonoBehaviour
{
    public Action<grid.TileMapType, Vector3Int, Color> OnChangeTilemapColor;
    public Action<grid.TileMapType, Vector3Int, TileBase> OnChangeTiemapTile;
    public Func<Vector3Int, bool> IsTileExistFurnitureAlready;

    public void DrawPreview(Vector3Int previewInstallPos, List<Vector3Int> previewInstallRange, TileBase selectedFurniture)
    {
        DrawPreviewInstallRange(previewInstallPos, previewInstallRange);
        DrawPreviewInstallFurniture(previewInstallPos, selectedFurniture);
    }

    private void DrawPreviewInstallRange(Vector3Int previewPos, List<Vector3Int> previewInstallRange)
    {
        foreach (var installRange in previewInstallRange)
        {
            OnChangeTilemapColor?.Invoke(grid.TileMapType.Floor, installRange, IsTileExistFurnitureAlready(installRange) ? Color.red : Color.green);
        }
    }
    private void DrawPreviewInstallFurniture(Vector3Int previewPos, TileBase selectedFurniture)
    {
        OnChangeTiemapTile?.Invoke(grid.TileMapType.FurniturePreview, previewPos, selectedFurniture);
    }
}