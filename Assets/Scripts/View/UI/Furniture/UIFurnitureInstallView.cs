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
    public Func<grid.TileMapType, Vector3Int, TileBase> GetTilemapTile;
    public Func<bool> OnInstallTile;

    // todo : installFurnitureModel에만 상태를 가지도록 하기
    public TileBase SelectedFurniture { get; set; }

    // todo : 전용 모델에 이동
    public List<Vector3Int> FurnitureInstallRanges { get; private set; } = new List<Vector3Int>();
    private Vector3Int furnitureInstallPos;

    public Action OnInstallFinish;

    public void ClearAndDraw(Vector3Int previewInstallPos)
    {
        if (!SelectedFurniture) return;
        if (furnitureInstallPos == previewInstallPos) return;

        ClearInstallPreview();
        DrawPreviewInstallRange(previewInstallPos);
        DrawPreviewInstallFurniture(previewInstallPos);
    }

    private void Update()
    {
        if (OnInstallTile())
        {
            ClearInstallPreview();
            InstallFurniture();
            OnInstallFinish?.Invoke();
            SelectedFurniture = null;
        }
    }

    public void ClearInstallPreview()
    {
        foreach (var installRange in FurnitureInstallRanges)
        {
            OnChangeTilemapColor?.Invoke(grid.TileMapType.Floor, installRange, Color.white);
        }
        OnChangeTiemapTile?.Invoke(grid.TileMapType.FurniturePreview, furnitureInstallPos, null);
    }
    public void DrawPreviewInstallRange(Vector3Int previewPos)
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
    public void DrawPreviewInstallFurniture(Vector3Int previewPos)
    {
        furnitureInstallPos = previewPos;
        OnChangeTiemapTile?.Invoke(grid.TileMapType.FurniturePreview, previewPos, SelectedFurniture);
    }
    private bool IsTileExistFurnitureAlready(Vector3Int gridPos)
    {
        return GetTilemapTile?.Invoke(grid.TileMapType.Furniture, gridPos) != null;
    }
    public void InstallFurniture()
    {
        OnChangeTiemapTile?.Invoke(grid.TileMapType.Furniture, furnitureInstallPos, SelectedFurniture);
    }
}