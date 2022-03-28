using UnityEngine;
using grid;

public class FurnitureController : BaseController
{
    private UIFurnitureStatusView uiFurnitureStatusView;
    private GridTilemapView gridTilemapView;
    private FurnitureManagerModel furnitureManagerModel;
    private FurnitureDecorateModel furnitureDecorateModel;

    // 테스트를 위해 임시로 할당
    // todo : UI에서 선택한 small object로부터 id를 취득하고, database에서 tilebase 값을 얻도록 하기.
    [SerializeField] int smallObjectId = 1;
    [SerializeField] UnityEngine.Tilemaps.TileBase tileBase;

    private int? selectFurnitureSerial;

    protected override void OnInitialize()
    {
        var gridInstallController = common.ControllerManager.instance.GetManagedController<GridInstallController>();
        gridTilemapView = gridInstallController.BuildDataHolder.gridTilemapView;
        uiFurnitureStatusView = common.ViewManager.instance.CreateViewObject<UIFurnitureStatusView>();

        modelInfoHolder.AddModel(out furnitureManagerModel);
        modelInfoHolder.AddModel(out furnitureDecorateModel);
    }

    protected override void OnFinalize()
    {
        // todo : view의 삭제(예약)
        // HogeView.FinalizeView();
        
        // todo : model의 삭제(참조 카운트 -1)
        // modelInfoHolder.RemoveModel(hogeModel)
    }

    private void OnEnable()
    {
        furnitureManagerModel.OnClickFurniture += OnClickFurniture;
        uiFurnitureStatusView.OnClickRotateButton += OnClickRotateButton;
        uiFurnitureStatusView.OnClickDecorateButton += OnClickDecorateButton;
    }
    private void OnDisable()
    {
        furnitureManagerModel.OnClickFurniture -= OnClickFurniture;
        uiFurnitureStatusView.OnClickRotateButton -= OnClickRotateButton;
        uiFurnitureStatusView.OnClickDecorateButton -= OnClickDecorateButton;
    }
    private void OnClickFurniture(FurnitureManagerObject furnitureManagerObject)
    {
        selectFurnitureSerial = furnitureManagerObject.Serial;
        var pos = furnitureManagerObject.FurnitureManagerGameObject.transform.position;
        uiFurnitureStatusView.Show(pos);
    }

    private void OnClickRotateButton()
    {
        furnitureManagerModel.ReverseFurnitureDirection(selectFurnitureSerial.Value);
    }

    private void OnClickDecorateButton()
    {
        // todo : smallObjectId를 UI를 통해 입력받기
        int furnitureId = furnitureManagerModel.GetIdFromSerial(selectFurnitureSerial.Value);
        var decorateInfo = furnitureDecorateModel.GetDecorateInfo(furnitureId, smallObjectId);
        if (decorateInfo.HasValue)
        {
            gridTilemapView.SetTile(TileMapType.Decorate, furnitureManagerModel.GetInstallPosFromSerial(selectFurnitureSerial.Value), tileBase);
            gridTilemapView.OffsetTile(TileMapType.Decorate, furnitureManagerModel.GetInstallPosFromSerial(selectFurnitureSerial.Value), decorateInfo.Value.offset);
        }
        else
        {
            Debug.LogError($"furnitureId:{furnitureId}, smallObjectId:{smallObjectId}의 데코레이션 정보는 정의되지 않았습니다");
        }
    }
}