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
        public FurnitureDirectionType FurnitureDirection { set; get; }
        public GameObject FurnitureManagerGameObject { private set; get; }
        public Vector3Int InstallPos { private set; get; } // 설치 기준 위치
        public List<Vector3Int> InstallRestrictAreas { private set; get; } // 설치 범위
        public Vector3Int InteractionPos { private set; get; } // 상호작용 기준 위치
        public FurnitureManagerObject(int id, GameObject furnitureManagerGameObject, Vector3Int installPos, List<Vector3Int> installRestrictAreas, Vector3Int interactionPos)
        {
            Id = id;
            Serial = serialMaker++;
            Collider = furnitureManagerGameObject.GetComponent<Collider2D>();
            priority = furnitureManagerGameObject.transform.position.y;
            FurnitureManagerGameObject = furnitureManagerGameObject;
            FurnitureDirection = FurnitureDirectionType.Left;
            InstallPos = installPos;
            InstallRestrictAreas = installRestrictAreas;
            InteractionPos = interactionPos;
        }

        public void UpdateDirection(FurnitureDirectionType directionType)
        {
            FurnitureDirection = directionType;

            // 좌우반전 상태일때는 x스케일을 -1
            int scaleX = directionType == FurnitureDirectionType.Left ? 1 : -1;
            FurnitureManagerGameObject.transform.localScale = new Vector3(scaleX, 1, 1);

            // 설치제한지역에 좌우반전 상태 적용
            for (int i = 0; i < InstallRestrictAreas.Count; ++i)
            {
                var reversedValue = ReverseLeftAndRight(InstallRestrictAreas[i], directionType);
                if (reversedValue.HasValue) InstallRestrictAreas[i] = reversedValue.Value;
            }

            // 상호작용 위치에 좌우반전 적용
            var reversedInteractionPosValue = ReverseLeftAndRight(InteractionPos, directionType);
            if(reversedInteractionPosValue.HasValue) InteractionPos = reversedInteractionPosValue.Value;
        }

        /// <summary>
        /// 해당 좌표 데이터를 설치 위치를 기준으로 좌우반전 시킴
        /// </summary>
        /// <param name="pos">반전 시킬 좌표 데이터</param>
        /// <param name="direction">적용 시킬 방향</param>
        /// <returns>좌우반전 처리가 되었을 경우엔 적용된 데이터를 반환. 처리가 불필요 했을 경우 null반환</returns>
        private Vector3Int? ReverseLeftAndRight(Vector3Int pos, FurnitureDirectionType direction)
        {
            var diff = pos - InstallPos;

            // 회전 중심축이 같기 때문에 처리가 불필요.
            // 사용자 기준으로 같은 y축상에 있음.
            if (diff.x == diff.y) return null;

            // 이미 해당 방향이므로 처리가 불필요.
            if (diff.x < diff.y && direction == FurnitureDirectionType.Left) return null;
            if (diff.x > diff.y && direction == FurnitureDirectionType.Right) return null;

            // 반전은 차분의 x,y를 반전시키고 기준점을 더한 값
            return new Vector3Int(diff.y + InstallPos.x, diff.x + InstallPos.y, pos.z);
        }
    };
}