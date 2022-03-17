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
        private static int serialMaker = 0; // 모든 가구가 다른 Serial를 가지도록 하기 위해 생성자에서 serial로 할당
        public int Id { private set; get; }
        public int Serial { private set; get; }
        public Collider2D Collider { private set; get; }
        public float priority { private set; get; } // y좌표와 동일
        public GameObject FurnitureManagerGameObject { private set; get; }
        public FurnitureDirectionType FurnitureDirection { set; get; }
        public Vector3Int InstallPos { private set; get; }
        public List<Vector3Int> InstallRestrictAreas { private set; get; } // 설치 범위
        public FurnitureManagerObject(int id, GameObject furnitureManagerGameObject, Vector3Int installPos, List<Vector3Int> installRestrictAreas)
        {
            Id = id;
            Serial = serialMaker++;
            Collider = furnitureManagerGameObject.GetComponent<Collider2D>();
            priority = furnitureManagerGameObject.transform.position.y;
            FurnitureManagerGameObject = furnitureManagerGameObject;
            FurnitureDirection = FurnitureDirectionType.Left;
            InstallPos = installPos;
            InstallRestrictAreas = installRestrictAreas;
        }
        public void UpdateDirection(FurnitureDirectionType directionType)
        {
            FurnitureDirection = directionType;

            // 좌우반전 상태일때는 x스케일을 -1
            int scaleX = directionType == FurnitureDirectionType.Left ? 1 : -1;
            FurnitureManagerGameObject.transform.localScale = new Vector3(scaleX, 1, 1);

            for (int i = 0; i < InstallRestrictAreas.Count; ++i)
            {
                var installRestrictArea = InstallRestrictAreas[i];

                var diff = installRestrictArea - InstallPos;

                // 회전 중심축이 같기 때문에 처리가 불필요.
                // 사용자 기준으로 같은 y축상에 있음.
                if(diff.x == diff.y) continue;

                // 이미 해당 방향이므로 처리가 불필요.
                if(diff.x < diff.y && directionType == FurnitureDirectionType.Left) continue;
                if(diff.x > diff.y && directionType == FurnitureDirectionType.Right) continue;

                // 반전은 차분의 x,y를 반전시키고 기준점을 더한 값
                InstallRestrictAreas[i] = new Vector3Int(diff.y + InstallPos.x, diff.x + InstallPos.y, installRestrictArea.z);
            }
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