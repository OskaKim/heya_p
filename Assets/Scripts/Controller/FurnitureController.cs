using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class FurnitureController : BaseController
{
    #region view
    [SerializeField] UIFurnitureStatusView uiFurnitureStatusView;
    [SerializeField] GridTilemapView gridTilemapView;
    #endregion

    #region controller
    #endregion

    #region model
    private FurnitureManagerModel furnitureManagerModel;
    private FurnitureDecorateModel furnitureDecorateModel;
    #endregion

    // 테스트를 위해 임시로 할당
    // todo : UI에서 선택한 small object로부터 id를 취득하고, database에서 tilebase 값을 얻도록 하기.
    [SerializeField] int smallObjectId = 1;
    [SerializeField] UnityEngine.Tilemaps.TileBase tileBase;

    private int? selectFurnitureSerial;

    protected override void SetupModels()
    {
        modelInfoHolder.AddModel(out furnitureManagerModel);
        modelInfoHolder.AddModel(out furnitureDecorateModel);
    }

    protected override void SetupViews()
    {
        furnitureManagerModel.OnClickFurniture += (FurnitureManagerObject furnitureManagerObject) =>
        {
            selectFurnitureSerial = furnitureManagerObject.Serial;
            var pos = furnitureManagerObject.FurnitureManagerGameObject.transform.position;
            uiFurnitureStatusView.Show(pos);
        };
        uiFurnitureStatusView.OnClickRotateButton += () =>
        {
            furnitureManagerModel.ReverseFurnitureDirection(selectFurnitureSerial.Value);
        };
        uiFurnitureStatusView.OnClickDecorateButton += () =>
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
        };
    }
}