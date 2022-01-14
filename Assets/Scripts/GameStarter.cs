using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// NOTE : 외부 데이터를 바탕으로 게임을 구성하는 클래스
public class GameStarter : MonoBehaviour
{

    // TODO : 외부 데이터나 세이브로부터 로드 되도록 하기
    [SerializeField] Vector3Int characterInitializePosition;

    [SerializeField] Grid grid;
    [SerializeField] GameObject characterPrefab;
    [SerializeField] Tilemap furnitureTilemap;
    [SerializeField] Tilemap floorTilemap;
    [SerializeField] TileBase[] furnitures;

    FurnitureInstallInfoHolder furnitureInstallInfoHolder;
    private void Start()
    {
        furnitureInstallInfoHolder = new FurnitureInstallInfoHolder();

        var worldPos = grid.CellToWorld(characterInitializePosition);
        GameObject.Instantiate(characterPrefab, worldPos, Quaternion.identity);

        // todo : furnitureInstallModel에서 설치 처리하도록 하기
        // todo : 외부 데이터에서 가구를 배치하도록
        furnitureTilemap.SetTile(new Vector3Int(2, 0, 0), furnitures[2]);
        furnitureTilemap.SetTile(new Vector3Int(1, 4, 0), furnitures[1]);
        furnitureTilemap.SetTile(new Vector3Int(-1, -1, 0), furnitures[0]);

        // todo : static을 통한 접근이 아니도록
        FurnitureInstallInfoHolder.InstallVectorInfo.Add(2, new Vector3Int(2, 0, 0));
        FurnitureInstallInfoHolder.InstallVectorInfo.Add(1, new Vector3Int(1, 4, 0));
        FurnitureInstallInfoHolder.InstallVectorInfo.Add(0, new Vector3Int(-1, -1, 0));
    }
}