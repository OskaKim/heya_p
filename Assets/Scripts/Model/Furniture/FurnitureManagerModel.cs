using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace grid
{
    public class FurnitureManagerModel : MonoBehaviour
    {
        private List<Collider2D> furnitureColliders = new List<Collider2D>();

        public void Setup()
        {
            this.UpdateAsObservable()
            .Select(_ => Camera.main.ScreenPointToRay(Input.mousePosition))
            .Subscribe(ray =>
            {
                var hit = Physics2D.Raycast(ray.origin, Vector3.zero, float.MaxValue, 3);
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);
                foreach (var furnitureCollider in furnitureColliders)
                {
                    // todo : 클릭시 처리
                    if (hit.collider == furnitureCollider) {
                        Debug.Log(hit.collider.name);
                    }
                }
            });
        }

        public void AddFurnitureColliders(Collider2D furnitureCollider)
        {
            furnitureColliders.Add(furnitureCollider);
        }
    }
}