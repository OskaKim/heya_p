using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/*
캐릭터 이동에 관련된 데이터
현재 위치, 앞으로의 루트 리스트
*/
public class CharacterMoveModel : MonoBehaviour
{
    private Grid grid;

    private void Awake() {
        grid = GameObject.FindObjectOfType<Grid>();    
    }

    public UniRx.ReactiveProperty<Vector3Int> GridPos
    {
        get; set;
    } = new ReactiveProperty<Vector3Int>();

    public Vector3Int NextGridPos
    {
        get
        {
            if (PathList.Count == 0) return GridPos.Value;
            return PathList.Peek();
        }
    }

    public Queue<Vector3Int> PathList
    {
        get; set;
    } = new Queue<Vector3Int>();

    public Vector3 GetWorldPosFrom(Vector3Int gridPos)
    {
        return grid.CellToWorld(gridPos);
    }

    public Vector3Int GetGridPosFrom(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    public void Goal()
    {
        if(PathList.Count > 0) PathList.Dequeue();
    }
}
