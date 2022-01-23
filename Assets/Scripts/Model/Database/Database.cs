using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataBase
{
    public class FurnitureData
    {
        public Sprite sprite;
        public int id;
    }

    // NOTE : 값이 정해진 static한 데이터의 집합. 참조는 모델에 한함
    public class Database : MonoBehaviour
    {
        public static List<FurnitureData> FurnitureDatabase = new List<FurnitureData> { };

        private void Awake()
        {
            InitializeFurnitureDatabase();
        }

        private void InitializeFurnitureDatabase()
        {
            FurnitureDatabase.Clear();
            
            int furnitureDataId = 0;
            foreach (var resourceSprite in Resources.LoadAll<Sprite>("Furniture"))
            {
                FurnitureData furnitureData = new FurnitureData();
                furnitureData.sprite = resourceSprite;
                furnitureData.id = furnitureDataId++;
                FurnitureDatabase.Add(furnitureData);
            }
        }
    }
}