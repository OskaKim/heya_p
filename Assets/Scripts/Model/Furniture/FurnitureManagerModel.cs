using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grid
{
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

        public FurnitureManagerObject AddfurnitureManagerObjects(int id, GameObject furnitureManagerGameObject, FurnitureDirectionType furnitureDirection, Vector3Int installPos, List<Vector3Int> installRanges)
        {
            FurnitureManagerObject furnitureManagerObject = new FurnitureManagerObject(id, furnitureManagerGameObject, installPos, installRanges);
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
        public void ReverseFurnitureDirection(int serial)
        {
            FurnitureManagerObject furnitureManagerObject = null;
            for (int i = 0; i < furnitureManagerObjects.Count; ++i)
            {
                if (furnitureManagerObjects[i].Serial == serial)
                {
                    furnitureManagerObject = furnitureManagerObjects[i];
                    break;
                }
            }

            if (furnitureManagerObject == null)
            {
                Debug.LogError($"id{serial}의 가구를 찾지 못했습니다");
                return;
            }

            var direction = furnitureManagerObject.FurnitureDirection == FurnitureDirectionType.Left ?
            FurnitureDirectionType.Right : FurnitureDirectionType.Left;
            SetFurnitureDirection(furnitureManagerObject, direction);
        }

        public Vector3Int GetInstallPos(int serial)
        {
            return furnitureManagerObjects.First(x => x.Serial == serial).InstallPos;
        }

        public List<Vector3Int> GetInstallRestrictAreas(int serial)
        {
            return furnitureManagerObjects.First(x => x.Serial == serial).InstallRestrictAreas;
        }

        public List<Vector3Int> GetAllInstallRestrictArea()
        {
            return furnitureManagerObjects.SelectMany(x => x.InstallRestrictAreas).ToList();
        }

        // note : 이미 설치된 타일이 타일 설치 영역에 있음
        public bool IsInInstallRestrictArea(Vector3Int checkArea)
        {
            return GetAllInstallRestrictArea().Any(x => x == checkArea);
        }

        // note : 이미 설치된 타일이 타일 설치 영역에 있음
        public bool IsInInstallRestrictArea(List<Vector3Int> checkAreas)
        {
            return checkAreas.Any(x => IsInInstallRestrictArea(x));
        }

        public int GetIdFrom(int serial)
        {
            return furnitureManagerObjects.First(x => x.Serial == serial).Id;
        }
    }
}