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

    // todo : 모델의 정보를 사용하도록 하기
    private Vector3Int furnitureInstallPos;
    private List<Vector3Int> installRangeCache = new List<Vector3Int>();

    public void DrawPreview(Vector3Int previewInstallPos, List<Vector3Int> previewInstallRange, TileBase selectedFurniture)
    {
        if (furnitureInstallPos == previewInstallPos) return;

        DrawPreviewInstallRange(previewInstallPos, previewInstallRange);
        DrawPreviewInstallFurniture(previewInstallPos, selectedFurniture);
    }

    // todo : furnitureInstallRanges를 모델로 옮긴 뒤에 프레젠터에서 타일맵 뷰의 함수를 통해 처리하도록 하기
    public void ClearInstallPreview()
    {
        foreach (var installRange in installRangeCache)
        {
            OnChangeTilemapColor?.Invoke(grid.TileMapType.Floor, installRange, Color.white);
        }
        OnChangeTiemapTile?.Invoke(grid.TileMapType.FurniturePreview, furnitureInstallPos, null);
    }
    private void DrawPreviewInstallRange(Vector3Int previewPos, List<Vector3Int> previewInstallRange)
    {
        ClearInstallPreview();
        installRangeCache = new List<Vector3Int>(previewInstallRange);
        foreach (var installRange in previewInstallRange)
        {
            OnChangeTilemapColor?.Invoke(grid.TileMapType.Floor, installRange, IsTileExistFurnitureAlready(installRange) ? Color.red : Color.green);
        }
    }
    private void DrawPreviewInstallFurniture(Vector3Int previewPos, TileBase selectedFurniture)
    {
        furnitureInstallPos = previewPos;
        OnChangeTiemapTile?.Invoke(grid.TileMapType.FurniturePreview, previewPos, selectedFurniture);
    }
}