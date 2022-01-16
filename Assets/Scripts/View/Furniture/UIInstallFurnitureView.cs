using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class UIInstallFurnitureView : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap furnitureTilemap;
    [SerializeField] private Tilemap furniturePrevieTilemap;
    [SerializeField] private TileBase tempTileBase;
    [SerializeField] Transform ScrollViewContentRoot;
    [SerializeField] Transform ScrollViewContentPrefab;

    private List<Vector3Int> furnitureInstallRanges = new List<Vector3Int>();
    private Vector3Int furnitureInstallPos;
    private bool isInstallMode = false;
    private void Start()
    {
        foreach (var resourceSprite in Resources.LoadAll<Sprite>("Furniture"))
        {
            var content = Transform.Instantiate(ScrollViewContentPrefab, ScrollViewContentRoot);
            var contentImageComponent = content.GetComponent<Image>();
            contentImageComponent.sprite = resourceSprite;
            var size = resourceSprite.bounds.size;
            var ratio = size.x / size.y;
            var contentAspectRatioFitter = content.GetComponent<AspectRatioFitter>();
            contentAspectRatioFitter.aspectRatio = ratio;
        }
        var tilemapTouchHandler = Utility.InputUtility.GetTilemapTouchHandler(floorTilemap);
        tilemapTouchHandler.OnStayTimemap.Subscribe(installPos => OnStayTile(installPos));
    }

    private void OnStayTile(Vector3Int previewInstallPos)
    {
        if (!isInstallMode) return;
        if (furnitureInstallPos == previewInstallPos) return;

        ClearInstallPreview();
        DrawPreviewInstallRange(previewInstallPos);
        DrawPreviewInstallFurniture(previewInstallPos);
    }
    private void ClearInstallPreview()
    {
        if (furnitureInstallRanges.Count > 0)
        {
            foreach (var installRange in furnitureInstallRanges)
            {
                floorTilemap.SetColor(installRange, Color.white);
            }
            furnitureInstallRanges.Clear();
        }

        furniturePrevieTilemap.SetTile(furnitureInstallPos, null);
    }
    private void DrawPreviewInstallRange(Vector3Int previewPos)
    {
        // todo : 각 가구에 대응하는 위치를 적용 시켜야 함
        // todo : 외부에서 각 가구의 배치시 범위를 지정
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x - 1, previewPos.y - 1, 0));
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x - 1, previewPos.y, 0));
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x - 1, previewPos.y + 1, 0));
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x, previewPos.y - 1, 0));
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x, previewPos.y, 0));
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x, previewPos.y + 1, 0));
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x + 1, previewPos.y - 1, 0));
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x + 1, previewPos.y, 0));
        furnitureInstallRanges.Add(new Vector3Int(previewPos.x + 1, previewPos.y + 1, 0));

        foreach (var installRange in furnitureInstallRanges)
        {
            floorTilemap.SetTileFlags(installRange, TileFlags.None);
            floorTilemap.SetColor(installRange, IsTileExistFurnitureAlready(installRange) ? Color.red : Color.green);
        }
    }
    private void DrawPreviewInstallFurniture(Vector3Int previewPos)
    {
        furnitureInstallPos = previewPos;
        furniturePrevieTilemap.SetTileFlags(furnitureInstallPos, TileFlags.None);
        furniturePrevieTilemap.SetTile(furnitureInstallPos, tempTileBase);
    }
    private bool IsTileExistFurnitureAlready(Vector3Int tile)
    {
        return furnitureTilemap.GetTile(tile) != null;
    }

    private void InstallFurniture()
    {
        furnitureTilemap.SetTile(furnitureInstallPos, tempTileBase);
    }
    // todo : 입력은 전용 클래스에서 관리할 예정
    private void Update()
    {
        // todo : 임시
        if (isInstallMode && Input.GetMouseButtonDown(0))
        {
            ClearInstallPreview();
            InstallFurniture();
            isInstallMode = false;
        }

        // todo : 임시
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInstallMode = !isInstallMode;
        }
    }
}
