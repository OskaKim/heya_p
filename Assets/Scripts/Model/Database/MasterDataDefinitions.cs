using UnityEngine;
using UnityEngine.Tilemaps;

namespace DataBase
{
    // 게임에서 사용
    public struct FurnitureDataEntity
    {
        public int id;
        public Sprite sprite;
        public TileBase tile;
    }

    // json 데이터 형태
    [System.Serializable]
    public struct FurnitureDataJson
    {
        [System.Serializable]
        public struct FurnitureDataUnit
        {
            public int id;
            public string spritePath;
            public string tilePath;
        }

        public FurnitureDataUnit[] furnitureDataUnits;
    }
}