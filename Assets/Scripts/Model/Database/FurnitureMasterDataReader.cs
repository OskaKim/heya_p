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
            foreach (var furnitureDataUnit in furnitureDataJson.furnitureInfos)
            {
                FurnitureDataEntity furnitureData = new FurnitureDataEntity(furnitureDataUnit.id);
                furnitureData.sprite = Resources.Load<Sprite>(furnitureDataUnit.spritePath);
                furnitureData.tile = Resources.Load<TileBase>(furnitureDataUnit.tilePath);
                furnitureData.installRestrictedAreas = furnitureDataUnit.installRestrictedAreas.Select(data => new Vector3Int((int)data.x, (int)data.y, 0)).ToList();
                furnitureData.interactionPos = new Vector3Int((int)furnitureDataUnit.interactionPos.x, (int)furnitureDataUnit.interactionPos.y, 0);

                furnitureDatabase.Add(furnitureData);
            }

            foreach (var decorateInfo in furnitureDataJson.decorateInfos)
            {
                var furnitureDecorateInfos = furnitureDatabase.First(x => x.id == decorateInfo.id).decorateInfos;
                FurnitureDataEntity.DecorateInfo furnitureDecorateInfo = new FurnitureDataEntity.DecorateInfo();
                // 그리드 상 z는 y * 4 고정. 이걸 맞춰야 출력 우선순위가 일치하게 됨.
                float offsetZ = decorateInfo.decorateOffsetY * 4;
                furnitureDecorateInfo.offset = new Vector3(decorateInfo.decorateOffsetX, decorateInfo.decorateOffsetY, offsetZ);
                furnitureDecorateInfo.smallObjectId = decorateInfo.smallObjectId;
                furnitureDecorateInfos.Add(furnitureDecorateInfo);
            }
        }
    }
}