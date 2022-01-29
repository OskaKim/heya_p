using System.Collections;
using System.Collections.Generic;
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
            foreach (var furnitureDataUnit in furnitureDataJson.furnitureDataUnits)
            {
                FurnitureDataEntity furnitureData = new FurnitureDataEntity();
                furnitureData.id = furnitureDataUnit.id;
                furnitureData.sprite = Resources.Load<Sprite>(furnitureDataUnit.spritePath);
                furnitureData.tile = Resources.Load<TileBase>(furnitureDataUnit.tilePath);

                furnitureDatabase.Add(furnitureData);
            }
        }
    }
}