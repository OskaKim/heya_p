using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// todo
// 프로토타입을 위해 임시 대응 상태
public class FurnitureInstallInfoHolder : MonoBehaviour
{
    public Dictionary<int, Vector3Int> InstallVectorInfo
    {
        get; set;
    } = new Dictionary<int, Vector3Int>();

    public bool IsAnyFurniture(Vector3Int gridPos)
    {
        return InstallVectorInfo.Any(x => x.Value == gridPos);
    }
}
