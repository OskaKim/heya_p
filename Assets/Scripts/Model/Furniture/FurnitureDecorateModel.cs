using DataBase;
using UnityEngine;
using System.Linq;

public class FurnitureDecorateModel : BaseModel<FurnitureDecorateModel>
{

    public FurnitureDataEntity.DecorateInfo? GetDecorateInfo(int furnitureId, int smallObjectId)
    {
        var furnitureData = MasterDataHolder.FurnitureDatabase.First(x=>x.id == furnitureId);
        FurnitureDataEntity.DecorateInfo? decorateInfo = null;

        decorateInfo = furnitureData.decorateInfos.Cast<FurnitureDataEntity.DecorateInfo?>().FirstOrDefault(x=>x.Value.smallObjectId == smallObjectId);
        return decorateInfo;
    }
}