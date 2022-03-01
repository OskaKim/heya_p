using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

interface ICharacterBehaviour
{
    public void Update(out bool isFinished);
}

public class CharacterBehaviourAppetite : ICharacterBehaviour
{
    int count = 10;

    public void Update(out bool isFinished)
    {
        /// <summary>
        /// 냉장고 타입의 가구를 찾는다
        /// 냉장고가 없으면 : 미정. 일단 UI로 표시
        /// 냉장고가 있으면 : 그 가구의 사용 위치로 이동한다.
        /// 욕구를 충족하고 행동 종료
        /// </summary>
        isFinished = false;

        if (--count <= 0)
        {
            isFinished = true;
            Debug.Log("욕구 충족");
        }
    }
}
public class CharacterBehaviourParched : ICharacterBehaviour
{
    int count = 10;

    public void Update(out bool isFinished)
    {
        /// <summary>
        /// 냉장고 타입의 가구를 찾는다
        /// 냉장고가 없으면 : 미정. 일단 UI로 표시
        /// 냉장고가 있으면 : 그 가구의 사용 위치로 이동한다.
        /// 욕구를 충족하고 행동 종료
        /// </summary>
        isFinished = false;

        if (--count <= 0)
        {
            isFinished = true;
            Debug.Log("욕구 충족");
        }
    }
}

public class CharacterAIModel : BaseModel<CharacterAIModel>
{
    private Subject<string> onUpdateCharacterAIEmotion = new Subject<string>();
    public IObservable<string> OnUpdateCharacterAIEmotion { get => onUpdateCharacterAIEmotion; }
    Queue<ICharacterBehaviour> characterBehaviours = new Queue<ICharacterBehaviour>();

    // NOTE : 필수 불만 요소
    public enum EssentialComplaintType
    {
        Appetite,       // 배가 고픔
        Parched,        // 목이 마름
        Hot,            // 더움
        Cold,           // 추움
        SleepDeprived,  // 수면부족
    };

    public void AddEssentialComplaint(EssentialComplaintType essetialComplaintType)
    {
        switch (essetialComplaintType)
        {
            case EssentialComplaintType.Appetite:
                characterBehaviours.Enqueue(new CharacterBehaviourAppetite());
                break;
            case EssentialComplaintType.Parched:
                characterBehaviours.Enqueue(new CharacterBehaviourParched());
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    public void UpdateCharacterBehaviour()
    {
        if (characterBehaviours.Count == 0) return;


        var firstCharacterBehaviour = characterBehaviours.Peek();
        bool isFinished;

        // todo : 임시 대응
        if (firstCharacterBehaviour is CharacterBehaviourAppetite)
        {
            onUpdateCharacterAIEmotion.OnNext("I'm hungry");
        }
        // todo : 임시 대응
        else if (firstCharacterBehaviour is CharacterBehaviourParched)
        {
            onUpdateCharacterAIEmotion.OnNext("I'm Thirsty");
        }

        firstCharacterBehaviour.Update(out isFinished);

        if (isFinished)
        {
            Debug.Log("행동 종료");
            characterBehaviours.Dequeue();

            // todo : 임시 대응
            if (characterBehaviours.Count == 0)
            {
                onUpdateCharacterAIEmotion.OnNext("I'm good now");
            }
        }
    }
}
