using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Utility;

/*
TODO : 아직 테스트, 메모 단계
캐릭터 이동 로직.

*/
public class CharacterMoveViewModel : MonoBehaviour
{
    private CharacterMoveModel model;
    private void Start()
    {
        model = transform.GetComponent<CharacterMoveModel>();
        model.GridPos.Subscribe(curPos =>
        {
            // todo : 임시
            Debug.Log($"위치갱신:{curPos}");
        });

        this.UpdateAsObservable().Subscribe(_ =>
        {
            UpdateMoveToNextPosTick();
        });
    }
    private void CalcPathTo(Vector3Int gridDestination)
    {
        model.PathList.Clear();
        model.PathList = PathUtility.GetPath(GetCurrentGridPos(), gridDestination);
    }

    private Vector3Int GetCurrentGridPos()
    {
        return model.GridPos.Value;
    }

    private Vector3Int GetNextGridPos()
    {
        return model.NextGridPos;
    }

    private bool isGoalNextGridPos()
    {
        var nextPos = model.GetWorldPosFrom(GetNextGridPos());
        Vector3 offset = nextPos - transform.position;
        float sqrLen = offset.sqrMagnitude;

        return sqrLen < 0.1f;
    }

    private void UpdateMoveToNextPosTick()
    {
        var nextPos = model.GetWorldPosFrom(GetNextGridPos());
        var speed = 0.1f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed);

        if (isGoalNextGridPos())
        {
            var next = GetNextGridPos();
            Debug.Log($"goal, next was {next}");
            model.Goal();
        }
    }
}