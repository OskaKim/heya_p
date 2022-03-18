using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using System.Linq;
using DataBase;
using System.Collections.ObjectModel;

namespace grid
{
    public class InstallFurnitureModel : BaseModel<InstallFurnitureModel>
    {
        public ReactiveProperty<int> SelectedFurniture { get; private set; } = new ReactiveProperty<int>(-1);
        public ReactiveProperty<Vector3Int> InstallPos { get; private set; } = new ReactiveProperty<Vector3Int>();
        public List<Vector3Int> InstallRestrictedAreas { get; private set; } = new List<Vector3Int>();
        public ReadOnlyCollection<FurnitureDataEntity> FurnitureDataBase
        {
            get
            {
                return MasterDataHolder.FurnitureDatabase;
            }
        }
        private FurnitureDataEntity GetFurnitureData(int id)
        {
            return FurnitureDataBase.FirstOrDefault(x => x.id == id);
        }
        public Sprite GetFurnitureSprite(int id)
        {
            return GetFurnitureData(id).sprite;
        }
        public TileBase GetFurnitureTile(int id)
        {
            return GetFurnitureData(id).tile;
        }

        public TileBase GetSelectedFurnitureTile()
        {
            return GetFurnitureTile(SelectedFurniture.Value);
        }
        public bool ExistSelectedFurnitureTile()
        {
            return GetSelectedFurnitureTile() != null;
        }

        public TileMapType GetInstallTilemapType()
        {
            return TileMapType.Floor;
        }
        public void UpdateInstallPos(Vector3Int pos)
        {
            if (SelectedFurniture.HasValue && GetFurnitureTile(SelectedFurniture.Value) != null)
            {
                InstallPos.Value = pos;

                InstallRestrictedAreas = GetFurnitureData(SelectedFurniture.Value)
                .installRestrictedAreas
                .Select(data => new Vector3Int(InstallPos.Value.x + data.x, InstallPos.Value.y + data.y, 0))
                .ToList();
            }
        }

        public void InstallFurniture()
        {
            // note : 선택 해제
            SelectedFurniture.Value = -1;

            // FurnitureManagerModel에서 설치된 가구를 관리함
        }
    }
}