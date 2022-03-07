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

    private int? selectFurniture;

    protected override void SetupModels()
    {
        modelInfoHolder.AddModel(out furnitureManagerModel);
        modelInfoHolder.AddModel(out furnitureDecorateModel);
    }

    protected override void SetupViews()
    {
        furnitureManagerModel.OnClickFurniture += (FurnitureManagerObject furnitureManagerObject) => {
            selectFurniture = furnitureManagerObject.Id;
            var pos = furnitureManagerObject.FurnitureManagerGameObject.transform.position;
            uiFurnitureStatusView.Show(pos);
        };
        uiFurnitureStatusView.OnClickRotateButton += () => {
            furnitureManagerModel.ReverseFurnitureDirection(selectFurniture.Value);
        };
        uiFurnitureStatusView.OnClickDecorateButton += () => {
            // todo : smallObjectId를 UI를 통해 입력받기
            int smallObjectEntityId = 1;

            // todo : 클릭중인 가구로부터 가구의 데이터 아이디를 가져오기
            int furnitureDataEntityId = 1;

            var decorateOffset = furnitureDecorateModel.GetDecorateOffset(furnitureDataEntityId, smallObjectEntityId);
            Debug.Log(decorateOffset);

            gridTilemapView.SetTile(TileMapType.Decorate, furnitureManagerModel.GetInstallPos(selectFurniture.Value), tileBase);
            gridTilemapView.OffsetTile(TileMapType.Decorate, furnitureManagerModel.GetInstallPos(selectFurniture.Value), decorateOffset);
        };
    }
}