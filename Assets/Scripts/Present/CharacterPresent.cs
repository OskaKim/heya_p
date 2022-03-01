using System.Collections.Generic;
using UnityEngine;
using UniRx;
using grid;
using timeinfo;
using System;

public class CharacterPresent : BasePresent
{
    #region view
    [SerializeField] CharacterAIStatusUIView characterAIStatusUIView;
    #endregion

    #region controller
    #endregion

    #region model
    private CharacterAIModel characterAIModel;
    private TimeInfoModel timeInfoModel;
    #endregion

    // todo : 캐릭터 뷰 로써 관리하기
    [SerializeField] private GameObject characterGameObject;
    protected override void InitializeControllers()
    {
    }

    protected override void SetupModels()
    {
        characterAIModel = CharacterAIModel.instance;
        timeInfoModel = TimeInfoModel.instance;
        
        // todo : 임시로 상태 추가. 배고픔, 목마름
        characterAIModel.AddEssentialComplaint(CharacterAIModel.EssentialComplaintType.Appetite);
        characterAIModel.AddEssentialComplaint(CharacterAIModel.EssentialComplaintType.Parched);

        timeInfoModel.OnUpdateGameTime.Subscribe(_ =>
        {
            characterAIModel.UpdateCharacterBehaviour();
        });

        characterAIModel.OnUpdateCharacterAIEmotion.Subscribe(emotionText =>
        {
            characterAIStatusUIView.UpdateAIStatusText(emotionText);
        });
    }

    protected override void SetupViews()
    {
        characterAIStatusUIView.GetCharacterScreenPosition = () =>
        {
            return Camera.main.WorldToScreenPoint(characterGameObject.transform.position);
        };
    }
}