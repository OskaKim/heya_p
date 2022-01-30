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
    public class InstallFurnitureModel
    {
        public ReactiveProperty<int> SelectedFurniture { get; private set; } = new ReactiveProperty<int>(-1);
        public ReactiveProperty<Vector3Int> InstallPos { get; private set; } = new ReactiveProperty<Vector3Int>();
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
            if (SelectedFurniture.HasValue && GetFurnitureTile(SelectedFurniture.Value) != null) InstallPos.Value = pos;
        }

        public void UnSelectFurniture()
        {
            SelectedFurniture.Value = -1;
        }
    }
}