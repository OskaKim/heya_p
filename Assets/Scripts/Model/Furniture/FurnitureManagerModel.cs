using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace grid
{
    // NOTE : 각 가구를 관리하기 위한 구조체
    public struct FurnitureManagerObject
    {
        private static int uniqueIdMaker = 0; // 모든 가구가 다른 id를 가지도록 하기 위해 생성자에서 id로 할당
        public int Id { private set; get; }
        public Collider2D collider;

        public FurnitureManagerObject()
        {
            Id = uniqueIdMaker++;
            collider = null;
        }
    };

    public class FurnitureManagerModel : MonoBehaviour
    {
        private List<FurnitureManagerObject> furnitureManagerObjects = new List<FurnitureManagerObject>();

        public void Setup()
        {
            this.UpdateAsObservable()
            .Select(_ => Camera.main.ScreenPointToRay(Input.mousePosition))
            .Subscribe(ray =>
            {
                var hit = Physics2D.Raycast(ray.origin, Vector3.zero);
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);
                foreach (var furnitureCollider in furnitureManagerObjects)
                {
                    // todo : 클릭시 처리
                    if (hit.collider == furnitureCollider.collider)
                    {
                        
                    }
                }
            });
        }

        public void AddfurnitureManagerObjects(Collider2D furnitureCollider)
        {
            FurnitureManagerObject furnitureManagerObject = new FurnitureManagerObject();
            furnitureManagerObject.collider = furnitureCollider;
            furnitureManagerObjects.Add(furnitureManagerObject);
        }
    }
}