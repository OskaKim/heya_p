using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
using DataBase;

public class InstallFurnitureModel
{
    public ReactiveProperty<int> SelectedFurniture { get; set; } = new ReactiveProperty<int>(-1);
    public List<FurnitureData> FurnitureDataBase
    {
        get
        {
            return MasterData.FurnitureDatabase;
        }
    }
    private FurnitureData GetFurnitureData(int id)
    {
        return FurnitureDataBase.FirstOrDefault(x => x.id == id);
    }
    public TileBase GetFurnitureTile(int id)
    {
        return GetFurnitureData(id).tile;
    }
}