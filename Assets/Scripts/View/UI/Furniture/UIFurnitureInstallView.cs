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

    // todo : 전용 모델에 이동
    public List<Vector3Int> FurnitureInstallRanges { get; private set; } = new List<Vector3Int>();
    private Vector3Int furnitureInstallPos;

    public void ClearAndDraw(Vector3Int previewInstallPos, TileBase selectedFurniture)
    {
        if (furnitureInstallPos == previewInstallPos) return;

        ClearInstallPreview();
        DrawPreviewInstallRange(previewInstallPos);
        DrawPreviewInstallFurniture(previewInstallPos, selectedFurniture);
    }

    // todo : furnitureInstallRanges를 모델로 옮긴 뒤에 프레젠터에서 타일맵 뷰의 함수를 통해 처리하도록 하기
    public void ClearInstallPreview()
    {
        foreach (var installRange in FurnitureInstallRanges)
        {
            OnChangeTilemapColor?.Invoke(grid.TileMapType.Floor, installRange, Color.white);
        }
        OnChangeTiemapTile?.Invoke(grid.TileMapType.FurniturePreview, furnitureInstallPos, null);
    }
    private void DrawPreviewInstallRange(Vector3Int previewPos)
    {
        FurnitureInstallRanges.Clear();

        // todo : 각 가구에 대응하는 위치를 적용 시켜야 함
        // todo : 외부에서 각 가구의 배치시 범위를 지정
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x - 1, previewPos.y - 1, 0));
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x - 1, previewPos.y, 0));
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x - 1, previewPos.y + 1, 0));
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x, previewPos.y - 1, 0));
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x, previewPos.y, 0));
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x, previewPos.y + 1, 0));
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x + 1, previewPos.y - 1, 0));
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x + 1, previewPos.y, 0));
        FurnitureInstallRanges.Add(new Vector3Int(previewPos.x + 1, previewPos.y + 1, 0));

        foreach (var installRange in FurnitureInstallRanges)
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