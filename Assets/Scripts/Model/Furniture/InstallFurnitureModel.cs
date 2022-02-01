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
        public ReactiveProperty<List<Vector3Int>> InstallRange { get; private set; } = new ReactiveProperty<List<Vector3Int>>(new List<Vector3Int>());
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
            if (SelectedFurniture.HasValue && GetFurnitureTile(SelectedFurniture.Value) != null)
            {
                InstallPos.Value = pos;

                var installRangeValue = InstallRange.Value;
                installRangeValue.Clear();

                // todo : 각 가구에 대응하는 위치를 적용 시켜야 함
                // todo : 외부에서 각 가구의 배치시 범위를 지정
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x - 1, InstallPos.Value.y - 1, 0));
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x - 1, InstallPos.Value.y, 0));
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x - 1, InstallPos.Value.y + 1, 0));
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x, InstallPos.Value.y - 1, 0));
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x, InstallPos.Value.y, 0));
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x, InstallPos.Value.y + 1, 0));
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x + 1, InstallPos.Value.y - 1, 0));
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x + 1, InstallPos.Value.y, 0));
                installRangeValue.Add(new Vector3Int(InstallPos.Value.x + 1, InstallPos.Value.y + 1, 0));
            }
        }

        public void UnSelectFurniture()
        {
            SelectedFurniture.Value = -1;
        }
    }
}