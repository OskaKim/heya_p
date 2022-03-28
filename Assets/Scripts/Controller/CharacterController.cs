using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System.Linq;
public class CharacterController : BaseController
{
    private CharacterAIStatusUIView characterAIStatusUIView;
    private GridTilemapView gridTilemapView;
    private CharacterView characterView;
    private CharacterAIModel characterAIModel;
    private FurnitureManagerModel furnitureManagerModel;
    private TimeInfoModel timeInfoModel;


    protected override void Start()
    {
        // todo : 다른 컨트롤러에서 어떠한 경로로 뷰를 참조하도록 하기
        gridTilemapView = GameObject.FindObjectOfType<GridTilemapView>();
        
        characterAIStatusUIView = common.ViewManager.instance.CreateViewObject<CharacterAIStatusUIView>();
        characterView = common.ViewManager.instance.CreateViewObject<CharacterView>();
        
        modelInfoHolder.AddModel(out characterAIModel);
        modelInfoHolder.AddModel(out furnitureManagerModel);
        modelInfoHolder.AddModel(out timeInfoModel);

        // todo : 임시로 상태 추가. 배고픔, 목마름
        characterAIModel.AddEssentialComplaint(CharacterAIModel.EssentialComplaintType.Appetite);
        characterAIModel.AddEssentialComplaint(CharacterAIModel.EssentialComplaintType.Parched);

        // 상호작용 위치 취득 테스트

        timeInfoModel.OnUpdateGameTime.Subscribe(_ =>
        {
            characterAIModel.UpdateCharacterBehaviour();
        });

        characterAIModel.OnUpdateCharacterAIEmotion.Subscribe(emotionText =>
        {
            characterAIStatusUIView.UpdateAIStatusText(emotionText);
        });

        characterAIStatusUIView.GetCharacterScreenPosition = () =>
        {
            return characterView.GetCharacterUIPosition();
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // todo : ai 업데이트시 사용. 캐릭터를 이 위치로 이동 시키게하기
            var pos = gridTilemapView.GetTileWorldPos(TileMapType.Furniture, getFurnitureInteractionPos("ComplaintAppetite").Value, true);
            characterView.UpdateCharacterPos(pos);
        }
    }

    private Vector3Int? getFurnitureInteractionPos(string targetInteraction)
    {
        var targetFurnituresIds = DataBase.MasterDataHolder.FurnitureDatabase
        .Where(x => x.interactions.Any(interaction => interaction == targetInteraction))
        .Select(x => x.id)
        .ToList();

        var targetFurnitureSerials = targetFurnituresIds
        .SelectMany(x => furnitureManagerModel.GetSerialsFromId(x))
        .ToList();

        if (targetFurnitureSerials.Count == 0) return null;

        // todo : targetFurnitureSerials로 부터 랜덤 또는 규칙으로 하나의 가구만 추출
        return furnitureManagerModel
        .GetFurnitureManagerObjectFromSerial(targetFurnitureSerials[0])
        .InteractionPos;
    }
}