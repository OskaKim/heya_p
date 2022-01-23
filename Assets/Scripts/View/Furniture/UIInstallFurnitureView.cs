using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

// todo : 동작에 대한 부분을 model로 분리
public class UIInstallFurnitureView : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap furnitureTilemap;
    [SerializeField] private Tilemap furniturePrevieTilemap;
    [SerializeField] private TileBase tempTileBase;

    // todo : 전용 모델에 이동
    public List<Vector3Int> FurnitureInstallRanges { get; private set; } = new List<Vector3Int>();
    private Vector3Int furnitureInstallPos;

    public Action OnInstallFinish;

    public bool IsInstallMode { get; set; }
    private void Start()
    {
        var tilemapTouchHandler = Utility.InputUtility.GetTilemapTouchHandler(floorTilemap);
        tilemapTouchHandler.OnStayTimemap.Subscribe(installPos => OnStayTile(installPos));
    }

    private void OnStayTile(Vector3Int previewInstallPos)
    {
        if (!IsInstallMode) return;
        if (furnitureInstallPos == previewInstallPos) return;

        ClearInstallPreview();
        DrawPreviewInstallRange(previewInstallPos);
        DrawPreviewInstallFurniture(previewInstallPos);
    }
    // todo : 입력은 전용 클래스에서 관리할 예정
    private void Update()
    {
        // todo : 임시
        if (IsInstallMode && Input.GetMouseButtonDown(0))
        {
            ClearInstallPreview();
            InstallFurniture();
            OnInstallFinish?.Invoke();
        }
    }

    public void ClearInstallPreview()
    {
        foreach (var installRange in FurnitureInstallRanges)
        {
            floorTilemap.SetColor(installRange, Color.white);
        }

        furniturePrevieTilemap.SetTile(furnitureInstallPos, null);
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
            floorTilemap.SetTileFlags(installRange, TileFlags.None);
            floorTilemap.SetColor(installRange, IsTileExistFurnitureAlready(installRange) ? Color.red : Color.green);
        }
    }
    public void DrawPreviewInstallFurniture(Vector3Int previewPos)
    {
        furnitureInstallPos = previewPos;
        furniturePrevieTilemap.SetTileFlags(previewPos, TileFlags.None);
        furniturePrevieTilemap.SetTile(previewPos, tempTileBase);
    }
    private bool IsTileExistFurnitureAlready(Vector3Int gridPos)
    {
        return furnitureTilemap.GetTile(gridPos) != null;
    }
    public void InstallFurniture()
    {
        furnitureTilemap.SetTile(furnitureInstallPos, tempTileBase);
    }
}