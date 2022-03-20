using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtility
{
    // todo : y가 마이너스일 경우 제대로 보정이 안 되기 때문에 로직을 수정
    // note : 출력 우선순위를 맞추기 위해 z포지션을 보정한 worldPosition을 반환
    public static Vector3 GetCorrectGridWorldPosition(Vector3 worldPosition) {
        return new Vector3(worldPosition.x, worldPosition.y, worldPosition.y * 4);
    }
}
