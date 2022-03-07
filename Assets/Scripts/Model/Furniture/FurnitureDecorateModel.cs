using DataBase;
using UnityEngine;
using System.Linq;

public class FurnitureDecorateModel : BaseModel<FurnitureDecorateModel>
{

    public Vector3 GetDecorateOffset(int furnitureId, int smallObjectId)
    {
        var furnitureData = MasterDataHolder.FurnitureDatabase.First(x=>x.id == furnitureId);
        var decorateInfo = furnitureData.decorateInfos.First(x=>x.smallObjectId == smallObjectId);
        return decorateInfo.offset;
    }
}