using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace DataBase
{
    // NOTE : 게임 내에서 사용하는 static한 마스터 데이터의 집합. 초기화는 Awake단계에서 완결됨
    public class MasterDataHolder : MonoBehaviour
    {
        public static ReadOnlyCollection<FurnitureDataEntity> FurnitureDatabase { get => furnitureDatabase.AsReadOnly(); }
        public static TimeInfoDataEntity TimeInfoDatabase { get => timeInfoDatabase; }
        private static List<FurnitureDataEntity> furnitureDatabase;
        private static TimeInfoDataEntity timeInfoDatabase;
        private GameObject masterDataReaderHolder = null;

        private void Awake()
        {
            masterDataReaderHolder = new GameObject("MasterDataReaderHolder");
            InitializeFurnitureDatabase();
            InitializeTimeInfoDatabase();
            Destroy(masterDataReaderHolder);
        }

        private void InitializeFurnitureDatabase()
        {
            var furnitureMasterDataReader = masterDataReaderHolder.AddComponent<FurnitureMasterDataReader>();
            furnitureMasterDataReader.setup(out furnitureDatabase);
        }
        
        private void InitializeTimeInfoDatabase()
        {
            var timeInfoDataReader = masterDataReaderHolder.AddComponent<TimeInfoDataReader>();
            timeInfoDataReader.setup(out timeInfoDatabase);
        }
    }
}