using UnityEngine;

public class FurnitureInstallModel : MonoBehaviour {
    private FurnitureInstallInfoHolder furnitureInstallInfoHolder;

    public bool IsAnyFurniture(Vector3Int gridPos){
        return furnitureInstallInfoHolder.IsAnyFurniture(gridPos);
    }
}