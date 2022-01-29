using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
using DataBase;
using System.Collections.ObjectModel;

public class InstallFurnitureModel
{
    public ReactiveProperty<int> SelectedFurniture { get; set; } = new ReactiveProperty<int>(-1);
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
}