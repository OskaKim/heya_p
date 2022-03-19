using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

// todo: 타입별로 파일을 나눠서 관리하기
namespace DataBase
{
    // 게임에서 사용
    public struct FurnitureDataEntity
    {
        public int id;
        public Sprite sprite;
        public TileBase tile;
        public List<Vector3Int> installRestrictedAreas;
        public Vector3Int interactionPos;
        public List<string> interactions;

        public struct DecorateInfo
        {
            public int smallObjectId;
            public Vector3 offset;
        }

        public List<DecorateInfo> decorateInfos;

        public FurnitureDataEntity(int id)
        {
            this.id = id;
            sprite = null;
            tile = null;
            installRestrictedAreas = new List<Vector3Int>();
            interactionPos = new Vector3Int();
            decorateInfos = new List<DecorateInfo>();
            interactions = new List<string>();
        }
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
        public struct FurnitureInfo
        {
            public int id;
            public string spritePath;
            public string tilePath;
            public Vector2[] installRestrictedAreas;
            public Vector2 interactionPos;
        }

        [System.Serializable]
        public struct DecorateInfo
        {
            public int id;
            public int smallObjectId;
            public float decorateOffsetX;
            public float decorateOffsetY;
        }

        [System.Serializable]
        public struct InteractionInfo
        {
            public int id;
            public string[] interactions;
        }
        public FurnitureInfo[] furnitureInfos;
        public DecorateInfo[] decorateInfos;
        public InteractionInfo[] interactionInfos;
    }

    [System.Serializable]
    public struct TimeInfoDataJson
    {
        public double currentTime;
        public float timeIntervalSpeed;
    }
}