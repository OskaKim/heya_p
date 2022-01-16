using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utility
{
    public class TilemapTouchHandler : MonoBehaviour
    {
        private TilemapCollider2D inputTouchTilemapCollider = null;
        private Tilemap inputTouchTilemap = null;

        private Subject<Vector3Int> onStayTilemap = new Subject<Vector3Int>();
        private Subject<Vector3Int> onTouchDownTilemap = new Subject<Vector3Int>();
        private Subject<Vector3Int> onTouchPressTilemap = new Subject<Vector3Int>();
        private Subject<Vector3Int> onTouchUpTilemap = new Subject<Vector3Int>();

        // NOTE : 마우스 포인터가 타일맵 위에 위치. 어떤 터치 상황이든 호출 됨.
        public IObservable<Vector3Int> OnStayTimemap { get => onStayTilemap; }

        // NOTE : 터치를 한 순간
        public IObservable<Vector3Int> OnTouchDownTilemap { get => onTouchDownTilemap; }

        // NOTE : 터치중
        public IObservable<Vector3Int> OnTouchPressTilemap { get => onTouchPressTilemap; }

        // NOTE : 터치가 중단된 순간
        public IObservable<Vector3Int> OnTouchUpTilemap { get => onTouchUpTilemap; }

        public static TilemapTouchHandler CreateComponent(GameObject targetObject, Tilemap tilemap)
        {
            var component = targetObject.AddComponent<TilemapTouchHandler>();
            component.Init(tilemap);
            return component;
        }
        
        public void Init(Tilemap tilemap)
        {
            this.inputTouchTilemap = tilemap;
            this.inputTouchTilemapCollider = tilemap.GetComponent<TilemapCollider2D>();
        }

        private void Start() => this.UpdateAsObservable()
            .Select(_ => Camera.main.ScreenPointToRay(Input.mousePosition))
            .Where(ray =>
            {
                var hit = Physics2D.Raycast(ray.origin, Vector3.zero);
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);
                return hit.collider == inputTouchTilemapCollider;
            })
            .Subscribe(ray =>
            {
                var worldPosition = ray.GetPoint(-ray.origin.z / ray.direction.z);
                var gridPosition = inputTouchTilemap.WorldToCell(worldPosition);
                onStayTilemap.OnNext(gridPosition);

                if (Input.GetMouseButton(0))
                {
                    onTouchPressTilemap.OnNext(gridPosition);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    onTouchDownTilemap.OnNext(gridPosition);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    onTouchUpTilemap.OnNext(gridPosition);
                }
            });
    }
}
