using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// todo
// 프로토타입을 위해 임시 대응 상태
public class FurnitureInstallInfoHolder
{
    // todo : static가 아니도록 되야 함. 지금은 테스트 용으로 임시로 설정
    public static Dictionary<int, Vector3Int> InstallVectorInfo
    {
        get; set;
    } = new Dictionary<int, Vector3Int>();

    // todo : static가 아니도록 되야 함. 지금은 테스트 용으로 임시로 설정
    public static bool IsAnyFurniture(Vector3Int gridPos)
    {
        return InstallVectorInfo.Any(x => x.Value == gridPos);
    }
}
