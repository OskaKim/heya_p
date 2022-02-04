using UnityEngine;
using UnityEngine.Tilemaps;
using System;

// todo: 타입별로 파일을 나눠서 관리하기
namespace DataBase
{
    // 게임에서 사용
    public struct FurnitureDataEntity
    {
        public int id;
        public Sprite sprite;
        public TileBase tile;
    }

    public struct TimeInfoDataEntity
    {
        public DateTime currentTime;
        public float timeIntervalSpeed;
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

    [System.Serializable]
    public struct TimeInfoDataJson
    {
        public double currentTime;
        public float timeIntervalSpeed;
    }
}