// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UniRx;

// /*
// TODO : 캐릭터 이동을 게임에 넣을지는 보류가 되었기 때문에 일단 주석처리.
// 완전히 안 쓰게 된다면 삭제 예정.
// 캐릭터 이동에 관련된 데이터
// 현재 위치, 앞으로의 루트 리스트
// */
// public class CharacterMoveModel : MonoBehaviour
// {
//     private Grid grid;

//     private void Awake()
//     {
//         grid = GameObject.FindObjectOfType<Grid>();
//     }

//     public UniRx.ReactiveProperty<Vector3Int> GridPos
//     {
//         get; set;
//     } = new ReactiveProperty<Vector3Int>();

//     public Vector3Int NextGridPos
//     {
//         get
//         {
//             if (PathList.Count == 0) return GridPos.Value;
//             return PathList.Peek();
//         }
//     }

//     public Queue<Vector3Int> PathList
//     {
//         get; set;
//     } = new Queue<Vector3Int>();

//     public Vector3 GetWorldPosFrom(Vector3Int gridPos)
//     {
//         return grid.CellToWorld(gridPos);
//     }

//     public Vector3Int GetGridPosFrom(Vector3 worldPos)
//     {
//         return grid.WorldToCell(worldPos);
//     }

//     public bool Goal()
//     {
//         if (PathList.Count > 0) PathList.Dequeue();

//         return PathList.Count == 0;
//     }
// }
