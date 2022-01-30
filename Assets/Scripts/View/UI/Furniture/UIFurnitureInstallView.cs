using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

// todo : 동작에 대한 부분을 model로 분리
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

    // todo : furnitureInstallRanges를 모델로 옮긴 뒤에 프레젠터에서 타일맵 뷰의 함수를 통해 처리하도록 하기
    public void ClearInstallPreview(Vector3Int installPosCache, List<Vector3Int> installRangeCache)
    {
        foreach (var installRange in installRangeCache)
        {
            OnChangeTilemapColor?.Invoke(grid.TileMapType.Floor, installRange, Color.white);
        }
        OnChangeTiemapTile?.Invoke(grid.TileMapType.FurniturePreview, installPosCache, null);
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