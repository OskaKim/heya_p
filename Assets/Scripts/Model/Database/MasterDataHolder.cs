using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace DataBase
{

    // NOTE : 값이 정해진 static한 데이터의 집합. 참조는 모델에 한함
    public class MasterDataHolder : MonoBehaviour
    {
        public static ReadOnlyCollection<FurnitureDataEntity> FurnitureDatabase { get => furnitureDatabase.AsReadOnly(); }
        private static List<FurnitureDataEntity> furnitureDatabase;
        private GameObject masterDataReaderHolder = null;

        private void Awake()
        {
            masterDataReaderHolder = new GameObject("MasterDataReaderHolder");
            InitializeFurnitureDatabase();
            Destroy(masterDataReaderHolder);
        }

        private void InitializeFurnitureDatabase()
        {
            var furnitureMasterDataReader = masterDataReaderHolder.AddComponent<FurnitureMasterDataReader>();
            furnitureMasterDataReader.setup(out furnitureDatabase);
        }
    }
}