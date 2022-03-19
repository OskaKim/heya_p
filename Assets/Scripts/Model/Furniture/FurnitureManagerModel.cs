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

        #region setup
        public void Setup()
        {
            SetupFurnitureClickDelegate();
        }
        private void SetupFurnitureClickDelegate()
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
        #endregion

        #region furniture manager object
        // 가구 관리 오브젝트의 추가
        public FurnitureManagerObject AddfurnitureManagerObjects(int id, GameObject furnitureManagerGameObject, FurnitureDirectionType furnitureDirection, Vector3Int installPos)
        {
            var furnitureDataBase = DataBase.MasterDataHolder.FurnitureDatabase.FirstOrDefault(x => x.id == id);

            // 데이터베이스의 위치 + 설치 기준 위치
            var interactionPos = new Vector3Int(installPos.x + furnitureDataBase.interactionPos.x, installPos.y + furnitureDataBase.interactionPos.y, 0);
            var installRestrictedAreas = furnitureDataBase.installRestrictedAreas
                .Select(data => new Vector3Int(installPos.x + data.x, installPos.y + data.y, 0))
                .ToList();

            FurnitureManagerObject furnitureManagerObject = new FurnitureManagerObject(id, furnitureManagerGameObject, installPos, installRestrictedAreas, interactionPos);
            furnitureManagerObjects.Add(furnitureManagerObject);
            UpdateFurnitureDirection(furnitureManagerObject, furnitureDirection);
            return furnitureManagerObject;
        }
        #endregion

        #region direction
        // 가구 방향 갱신
        private void UpdateFurnitureDirection(FurnitureManagerObject furnitureManagerObject, FurnitureDirectionType furnitureDirection)
        {
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
            UpdateFurnitureDirection(furnitureManagerObject, direction);
        }
        #endregion

        #region restrictArea
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

        #endregion

        #region getter
        public int GetIdFromSerial(int serial)
        {
            return furnitureManagerObjects.First(x => x.Serial == serial).Id;
        }

        public Vector3Int GetInstallPosFromSerial(int serial)
        {
            return furnitureManagerObjects.First(x => x.Serial == serial).InstallPos;
        }
        #endregion

    }
}