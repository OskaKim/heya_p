using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTouchHandler : MonoBehaviour
{
    // NOTE : 터치용 타일맵.
    // 타일이 여러 개가 존재할 수 있지만 gridPosition은 동일하기 때문에 터치 조작용 타일은 하나로 통일
    [SerializeField] private TilemapCollider2D iputTouchTilemapCollider;
    [SerializeField] private Tilemap inputTouchTilemap;

    private Subject<Vector3Int> onTouchTilemap = new Subject<Vector3Int>();
    public IObservable<Vector3Int> OnTouchTilemap { get => onTouchTilemap; }
    private void Start() =>
    this.UpdateAsObservable()
    .Where(_ => Input.GetMouseButtonDown(0))
    .Select(_ => Camera.main.ScreenPointToRay(Input.mousePosition))
    .Where(ray =>
    {
        var hit = Physics2D.Raycast(ray.origin, Vector3.zero);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);
        return hit.collider == iputTouchTilemapCollider;
    })
    .Subscribe(ray =>
    {
        var worldPosition = ray.GetPoint(-ray.origin.z / ray.direction.z);
        var gridPosition = inputTouchTilemap.WorldToCell(worldPosition);
        Debug.Log(gridPosition);
        onTouchTilemap.OnNext(gridPosition);
    });
}
