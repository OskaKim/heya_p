using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grid
{
    public enum FurnitureDirectionType
    {
        Left,
        Right
    };

    // NOTE : 각 가구를 관리하기 위한 구조체
    public struct FurnitureManagerObject
    {
        private static int uniqueIdMaker = 0; // 모든 가구가 다른 id를 가지도록 하기 위해 생성자에서 id로 할당
        public int Id { private set; get; }
        public Collider2D Collider { private set; get; }
        public float priority { private set; get; } // y좌표와 동일
        public GameObject FurnitureManagerGameObject { private set; get; }
        public FurnitureDirectionType FurnitureDirection { private set; get; }
        public FurnitureManagerObject(GameObject furnitureManagerGameObject, FurnitureDirectionType furnitureDirection)
        {
            Id = uniqueIdMaker++;
            Collider = furnitureManagerGameObject.GetComponent<Collider2D>();
            priority = furnitureManagerGameObject.transform.position.y;
            FurnitureManagerGameObject = furnitureManagerGameObject;
            FurnitureDirection = furnitureDirection;
        }
    };

    public class FurnitureManagerModel : MonoBehaviour
    {
        private List<FurnitureManagerObject> furnitureManagerObjects = new List<FurnitureManagerObject>();
        public event Action<FurnitureManagerObject> OnClickFurniture;
        public void Setup()
        {
            this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => Camera.main.ScreenPointToRay(Input.mousePosition))
            .Subscribe(ray =>
            {
                var hits = Physics2D.RaycastAll(ray.origin, Vector3.zero);
                FurnitureManagerObject? target = null;
                foreach (var hit in hits)
                {
                    foreach (var furnitureManagerObject in furnitureManagerObjects)
                    {
                        bool isClicked = hit.collider == furnitureManagerObject.Collider;
                        if (isClicked)
                        {
                            // 여러개의 가구가 클릭되었을 경우, 아래쪽의 가구를 우선시 함
                            if (target == null || target.Value.priority > furnitureManagerObject.priority)
                            {
                                target = furnitureManagerObject;
                                break;
                            }
                        }
                    }
                }

                if (target.HasValue)
                {
                    OnClickFurniture?.Invoke(target.Value);
                }
            });
        }

        public FurnitureManagerObject AddfurnitureManagerObjects(GameObject furnitureManagerGameObject, FurnitureDirectionType furnitureDirection)
        {
            FurnitureManagerObject furnitureManagerObject = new FurnitureManagerObject(furnitureManagerGameObject, furnitureDirection);
            furnitureManagerObjects.Add(furnitureManagerObject);
            return furnitureManagerObject;
        }
    }
}