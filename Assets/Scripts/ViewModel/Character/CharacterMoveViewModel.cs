// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UniRx;
// using UniRx.Triggers;
// using Utility;

// /*
// TODO : 캐릭터 이동을 게임에 넣을지는 보류가 되었기 때문에 일단 주석처리.
// 완전히 안 쓰게 된다면 삭제 예정.
// 캐릭터 이동 로직.

// */
// public class CharacterMoveViewModel : MonoBehaviour
// {
//     [SerializeField] FurnitureInstallModel furnitureInstallModel;
//     private CharacterMoveModel model;
//     private void Start()
//     {
//         model = transform.GetComponent<CharacterMoveModel>();
//         model.GridPos.Subscribe(curPos =>
//         {
//             // todo : 임시
//             Debug.Log($"위치갱신:{curPos}");
//         });

//         // todo : 지우기
//         // 길 찾기 예시
//         // var x = UnityEngine.Random.Range(-5, 5);
//         // var y = UnityEngine.Random.Range(-5, 5);
//         // CalcPathTo(new Vector3Int(x, y, 0));

//         // this.UpdateAsObservable().Subscribe(_ =>
//         // {
//         //     UpdateMoveToNextPosTick();
//         // });
//     }

//     private void CalcPathTo(Vector3Int gridDestination)
//     {
//         model.PathList.Clear();
//         model.PathList = PathUtility.GetPath(GetCurrentGridPos(), gridDestination, furnitureInstallModel);
//     }

//     private Vector3Int GetCurrentGridPos()
//     {
//         return model.GridPos.Value;
//     }

//     private Vector3Int GetNextGridPos()
//     {
//         return model.NextGridPos;
//     }

//     private bool isGoalNextGridPos()
//     {
//         var nextPos = model.GetWorldPosFrom(GetNextGridPos());
//         Vector3 offset = nextPos - transform.position;
//         float sqrLen = offset.sqrMagnitude;

//         return sqrLen < 0.1f;
//     }

//     private void UpdateMoveToNextPosTick()
//     {
//         var nextPos = model.GetWorldPosFrom(GetNextGridPos());
//         var speed = 1.0f * Time.deltaTime;
//         transform.position = Vector2.MoveTowards(transform.position, nextPos, speed);

//         if (isGoalNextGridPos())
//         {
//             var next = GetNextGridPos();
//             Debug.Log($"goal, next was {next}");
//             if (model.Goal())
//             {
//                 var x = UnityEngine.Random.Range(-5, 5);
//                 var y = UnityEngine.Random.Range(-5, 5);
//                 CalcPathTo(new Vector3Int(x, y, 0));
//             }
//         }
//     }
// }