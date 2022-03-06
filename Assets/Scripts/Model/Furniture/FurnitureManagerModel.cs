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

    // NOTE : 각 가구를 관리하기 위한 클래스
    public class FurnitureManagerObject
    {
        private static int uniqueIdMaker = 0; // 모든 가구가 다른 id를 가지도록 하기 위해 생성자에서 id로 할당
        public int Id { private set; get; }
        public Collider2D Collider { private set; get; }
        public float priority { private set; get; } // y좌표와 동일
        public GameObject FurnitureManagerGameObject { private set; get; }
        public FurnitureDirectionType FurnitureDirection { set; get; }
        public Vector3Int InstallPos { private set; get; }
        public FurnitureManagerObject(GameObject furnitureManagerGameObject, Vector3Int installPos)
        {
            Id = uniqueIdMaker++;
            Collider = furnitureManagerGameObject.GetComponent<Collider2D>();
            priority = furnitureManagerGameObject.transform.position.y;
            FurnitureManagerGameObject = furnitureManagerGameObject;
            FurnitureDirection = FurnitureDirectionType.Left;
            InstallPos = installPos;
        }
        public void UpdateDirection(FurnitureDirectionType directionType)
        {
            FurnitureDirection = directionType;
        }
    };

    public class FurnitureManagerModel : BaseModel<FurnitureManagerModel>
    {
        private List<FurnitureManagerObject> furnitureManagerObjects = new List<FurnitureManagerObject>();
        public event Action<FurnitureManagerObject> OnClickFurniture;
        public event Action<Vector3Int, FurnitureDirectionType> OnRotateFurniture;
        public void Setup()
        {
            this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => Camera.main.ScreenPointToRay(Input.mousePosition))
            .Subscribe(ray =>
            {
                var hits = Physics2D.RaycastAll(ray.origin, Vector3.zero);
                FurnitureManagerObject target = null;
                foreach (var hit in hits)
                {
                    foreach (var furnitureManagerObject in furnitureManagerObjects)
                    {
                        bool isClicked = hit.collider == furnitureManagerObject.Collider;
                        if (isClicked)
                        {
                            // 여러개의 가구가 클릭되었을 경우, 아래쪽의 가구를 우선시 함
                            if (target == null || target.priority > furnitureManagerObject.priority)
                            {
                                target = furnitureManagerObject;
                                break;
                            }
                        }
                    }
                }

                if (target != null)
                {
                    OnClickFurniture?.Invoke(target);
                }
            });
        }

        public FurnitureManagerObject AddfurnitureManagerObjects(GameObject furnitureManagerGameObject, FurnitureDirectionType furnitureDirection, Vector3Int installPos)
        {
            FurnitureManagerObject furnitureManagerObject = new FurnitureManagerObject(furnitureManagerGameObject, installPos);
            furnitureManagerObjects.Add(furnitureManagerObject);
            SetFurnitureDirection(furnitureManagerObject, furnitureDirection);
            return furnitureManagerObject;
        }

        private void SetFurnitureDirection(FurnitureManagerObject furnitureManagerObject, FurnitureDirectionType furnitureDirection)
        {
            // todo : 설정한 방향으로 잘 설정이 안되는 버그 조사 후 대응
            furnitureManagerObject.UpdateDirection(furnitureDirection);
            OnRotateFurniture?.Invoke(furnitureManagerObject.InstallPos, furnitureManagerObject.FurnitureDirection);
        }

        // NOTE: 가구를 좌우 반전
        public void ReverseFurnitureDirection(int id)
        {
            FurnitureManagerObject furnitureManagerObject = null;
            for (int i = 0; i < furnitureManagerObjects.Count; ++i)
            {
                if (furnitureManagerObjects[i].Id == id)
                {
                    furnitureManagerObject = furnitureManagerObjects[i];
                    break;
                }
            }

            if(furnitureManagerObject == null) {
                Debug.LogError($"id{id}의 가구를 찾지 못했습니다");
                return;
            }
            
            var direction = furnitureManagerObject.FurnitureDirection == FurnitureDirectionType.Left ?
            FurnitureDirectionType.Right : FurnitureDirectionType.Left;
            SetFurnitureDirection(furnitureManagerObject, direction);
        }
    }
}