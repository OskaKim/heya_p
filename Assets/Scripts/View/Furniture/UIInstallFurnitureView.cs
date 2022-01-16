using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class UIInstallFurnitureView : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap furnitureTilemap;
    [SerializeField] private Tilemap furniturePrevieTilemap;
    [SerializeField] private TileBase tempTileBase;
    [SerializeField] private Transform ScrollViewContentRoot;
    [SerializeField] private Transform ScrollViewContentPrefab;

    private bool isInstallMode = false;
    private InstallFurnitureViewModel installFurnitureViewModel;

    private void Start()
    {
        foreach (var resourceSprite in Resources.LoadAll<Sprite>("Furniture"))
        {
            var content = Transform.Instantiate(ScrollViewContentPrefab, ScrollViewContentRoot);
            var contentImageComponent = content.GetComponent<Image>();
            contentImageComponent.sprite = resourceSprite;
            var size = resourceSprite.bounds.size;
            var ratio = size.x / size.y;
            var contentAspectRatioFitter = content.GetComponent<AspectRatioFitter>();
            contentAspectRatioFitter.aspectRatio = ratio;
        }
        var tilemapTouchHandler = Utility.InputUtility.GetTilemapTouchHandler(floorTilemap);
        tilemapTouchHandler.OnStayTimemap.Subscribe(installPos => OnStayTile(installPos));
        SetupInstallFurnitureViewModel();
    }

    private void SetupInstallFurnitureViewModel()
    {
        InstallFurnitureViewModel.Params modelParams;
        modelParams.furnitureTilemap = furnitureTilemap;
        modelParams.floorTilemap = floorTilemap;
        modelParams.previewTilemap = furniturePrevieTilemap;
        modelParams.selectedTile = tempTileBase;

        installFurnitureViewModel = new InstallFurnitureViewModel(modelParams);
    }
    private void OnStayTile(Vector3Int previewInstallPos)
    {
        if (!isInstallMode) return;
        if (installFurnitureViewModel.FurnitureInstallPos == previewInstallPos) return;

        installFurnitureViewModel.ClearInstallPreview();
        installFurnitureViewModel.DrawPreviewInstallRange(previewInstallPos);
        installFurnitureViewModel.DrawPreviewInstallFurniture(previewInstallPos);
    }
    // todo : 입력은 전용 클래스에서 관리할 예정
    private void Update()
    {
        // todo : 임시
        if (isInstallMode && Input.GetMouseButtonDown(0))
        {
            installFurnitureViewModel.ClearInstallPreview();
            installFurnitureViewModel.InstallFurniture();
            isInstallMode = false;
        }

        // todo : 임시
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInstallMode = !isInstallMode;
        }
    }
}
