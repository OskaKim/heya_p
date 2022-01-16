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
    private void Start()
    {
        var worldPos = grid.CellToWorld(characterInitializePosition);
        GameObject.Instantiate(characterPrefab, worldPos, Quaternion.identity);
    }
}