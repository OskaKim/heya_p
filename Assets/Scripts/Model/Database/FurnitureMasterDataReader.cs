using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DataBase
{
    public class FurnitureMasterDataReader : MonoBehaviour
    {
        public void setup(out List<FurnitureDataEntity> furnitureDatabase)
        {
            furnitureDatabase = new List<FurnitureDataEntity>();

            var furnitureDataJson = Utility.FileIOUtility.LoadJsonFile<FurnitureDataJson>("SaveFile/Test", "furnitureData");

            // 가구 기본 정보
            foreach (var furnitureDataUnit in furnitureDataJson.furnitureInfos)
            {
                FurnitureDataEntity furnitureData = new FurnitureDataEntity(furnitureDataUnit.id);
                furnitureData.sprite = Resources.Load<Sprite>(furnitureDataUnit.spritePath);
                furnitureData.tile = Resources.Load<TileBase>(furnitureDataUnit.tilePath);
                furnitureData.installRestrictedAreas = furnitureDataUnit.installRestrictedAreas.Select(data => new Vector3Int((int)data.x, (int)data.y, 0)).ToList();
                furnitureData.interactionPos = new Vector3Int((int)furnitureDataUnit.interactionPos.x, (int)furnitureDataUnit.interactionPos.y, 0);

                furnitureDatabase.Add(furnitureData);
            }

            // 데코레이트 정보
            foreach (var decorateInfo in furnitureDataJson.decorateInfos)
            {
                var furnitureDecorateInfos = furnitureDatabase.First(x => x.id == decorateInfo.id).decorateInfos;
                FurnitureDataEntity.DecorateInfo furnitureDecorateInfo = new FurnitureDataEntity.DecorateInfo();
                furnitureDecorateInfo.offset = GridUtility.GetCorrectGridWorldPosition(new Vector3(decorateInfo.decorateOffsetX, decorateInfo.decorateOffsetY, 0));
                furnitureDecorateInfo.smallObjectId = decorateInfo.smallObjectId;
                furnitureDecorateInfos.Add(furnitureDecorateInfo);
            }

            // 상호작용 정보
            foreach (var interactionInfo in furnitureDataJson.interactionInfos)
            {
                furnitureDatabase.First(x => x.id == interactionInfo.id).interactions.AddRange(interactionInfo.interactions.ToList());
            }
        }
    }
}