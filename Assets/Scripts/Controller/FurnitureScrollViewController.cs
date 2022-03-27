using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;

public class FurnitureScrollViewController : BaseController
{
    private UIFurnitureScrollViewView uiFurnitureScrollViewView;
    private InstallFurnitureModel installFurnitureModel;
    private FurnitureManagerModel furnitureManagerModel;

    protected override void Start()
    {
        // todo : view를 생성하고 컨트롤러에서 수명 관리
        uiFurnitureScrollViewView = GameObject.FindObjectOfType<UIFurnitureScrollViewView>();

        modelInfoHolder.AddModel(out installFurnitureModel);
        modelInfoHolder.AddModel(out furnitureManagerModel);
     
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
