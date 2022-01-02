using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTouchHandler : MonoBehaviour
{
    // NOTE : 터치용 타일맵.
    // 타일이 여러 개가 존재할 수 있지만 gridPosition은 동일하기 때문에 터치 조작용 타일은 하나로 통일
    [SerializeField] TilemapCollider2D iputTouchTilemapCollider;
    [SerializeField] Tilemap inputTouchTilemap;
    [SerializeField] Tilemap changeTouchTile;
    
    // TODO : unirx를 적용해서 인풋 이벤트로 작성
    private void Update()
    {
        try
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);
                var hit = Physics2D.Raycast(ray.origin, Vector3.zero);
                
                if (hit.collider == iputTouchTilemapCollider)
                {
                    var worldPosition = ray.GetPoint(-ray.origin.z / ray.direction.z);
                    var gridPosition = inputTouchTilemap.WorldToCell(worldPosition);

                    Debug.Log($"TouchedPosition:{gridPosition}");
                    
                    // test : get tile
                    changeTouchTile.SetTile(gridPosition, null);
                }
            }
        }
        catch (NullReferenceException)
        {
        }
    }
}
