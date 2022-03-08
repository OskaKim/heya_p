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

    [SerializeField] UnityEngine.Tilemaps.TileBase tileBase;

    private int? selectFurnitureSerial;

    protected override void SetupModels()
    {
        modelInfoHolder.AddModel(out furnitureManagerModel);
        modelInfoHolder.AddModel(out furnitureDecorateModel);
    }

    protected override void SetupViews()
    {
        furnitureManagerModel.OnClickFurniture += (FurnitureManagerObject furnitureManagerObject) => {
            selectFurnitureSerial = furnitureManagerObject.Serial;
            var pos = furnitureManagerObject.FurnitureManagerGameObject.transform.position;
            uiFurnitureStatusView.Show(pos);
        };
        uiFurnitureStatusView.OnClickRotateButton += () => {
            furnitureManagerModel.ReverseFurnitureDirection(selectFurnitureSerial.Value);
        };
        uiFurnitureStatusView.OnClickDecorateButton += () => {
            // todo : smallObjectId를 UI를 통해 입력받기
            int smallObjectEntityId = 1;
            int furnitureId = furnitureManagerModel.GetIdFrom(selectFurnitureSerial.Value);
            var decorateOffset = furnitureDecorateModel.GetDecorateOffset(furnitureId, smallObjectEntityId);

            gridTilemapView.SetTile(TileMapType.Decorate, furnitureManagerModel.GetInstallPos(selectFurnitureSerial.Value), tileBase);
            gridTilemapView.OffsetTile(TileMapType.Decorate, furnitureManagerModel.GetInstallPos(selectFurnitureSerial.Value), decorateOffset);
        };
    }
}