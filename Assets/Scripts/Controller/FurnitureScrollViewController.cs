using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;

public class FurnitureScrollViewController : BaseController
{
    private UIFurnitureScrollViewView uiFurnitureScrollViewView;
    private InstallFurnitureModel installFurnitureModel;
    private FurnitureManagerModel furnitureManagerModel;

    private void Awake()
    {
        uiFurnitureScrollViewView = common.ViewManager.instance.CreateViewObject<UIFurnitureScrollViewView>();

        modelInfoHolder.AddModel(out installFurnitureModel);
        modelInfoHolder.AddModel(out furnitureManagerModel);
    }

    private void Start()
    {
        UIFurnitureScrollViewView.Param param;

        param.furnitureScrollDataList = new List<UIFurnitureScrollViewView.Param.FurnitureScrollData>();

        foreach (var originData in installFurnitureModel.FurnitureDataBase)
        {
            UIFurnitureScrollViewView.Param.FurnitureScrollData uiScrollData;
            uiScrollData.sprite = originData.sprite;
            uiScrollData.id = originData.id;
            param.furnitureScrollDataList.Add(uiScrollData);
        }

        uiFurnitureScrollViewView.StartView(param);

        uiFurnitureScrollViewView.OnSelectFurniture.Subscribe(furnitureId =>
        {
            installFurnitureModel.SelectedFurniture.Value = furnitureId;
        });
    }
}
