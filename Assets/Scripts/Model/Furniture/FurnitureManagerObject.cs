using UnityEngine;
using System.Collections.Generic;

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
                if (diff.x == diff.y) continue;

                // 이미 해당 방향이므로 처리가 불필요.
                if (diff.x < diff.y && directionType == FurnitureDirectionType.Left) continue;
                if (diff.x > diff.y && directionType == FurnitureDirectionType.Right) continue;

                // 반전은 차분의 x,y를 반전시키고 기준점을 더한 값
                InstallRestrictAreas[i] = new Vector3Int(diff.y + InstallPos.x, diff.x + InstallPos.y, installRestrictArea.z);
            }
        }
    };
}