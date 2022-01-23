using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class InstallFurnitureModel
{
    public ReactiveProperty<int> SelectedFurniture { get; set; } = new ReactiveProperty<int>(-1);
    public ReactiveProperty<bool> FurnitureInstallModeToggle { get; set; } = new ReactiveProperty<bool>(false);
    public List<DataBase.FurnitureData> FurnitureDataBase
    {
        get
        {
            return DataBase.Database.FurnitureDatabase;
        }
    }
}