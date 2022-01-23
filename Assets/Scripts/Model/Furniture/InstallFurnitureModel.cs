using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
public class InstallFurnitureModel
{
    public List<Vector3Int> FurnitureInstallRanges { get; private set; } = new List<Vector3Int>();
    public Vector3Int FurnitureInstallPos { get; private set; }

    public struct Params
    {
        public Tilemap furnitureTilemap;
        public Tilemap floorTilemap;
        public Tilemap previewTilemap;
        public TileBase selectedTile;
    }

    private Params modelParams;

    public InstallFurnitureModel(Params modelParams)
    {
        this.modelParams = modelParams;
    }

    public void ClearInstallPreview()
    {
            foreach (var installRange in FurnitureInstallRanges)
            {
                modelParams.floorTilemap.SetColor(installRange, Color.white);
            }

            modelParams.previewTilemap.SetTile(FurnitureInstallPos, null);
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
            modelParams.floorTilemap.SetTileFlags(installRange, TileFlags.None);
            modelParams.floorTilemap.SetColor(installRange, IsTileExistFurnitureAlready(installRange) ? Color.red : Color.green);
        }
    }
    public void DrawPreviewInstallFurniture(Vector3Int previewPos)
    {
        FurnitureInstallPos = previewPos;
        modelParams.previewTilemap.SetTileFlags(previewPos, TileFlags.None);
        modelParams.previewTilemap.SetTile(previewPos, modelParams.selectedTile);
    }

    private bool IsTileExistFurnitureAlready(Vector3Int gridPos)
    {
        return modelParams.furnitureTilemap.GetTile(gridPos) != null;
    }
    public void InstallFurniture()
    {
        modelParams.furnitureTilemap.SetTile(FurnitureInstallPos, modelParams.selectedTile);
    }
}