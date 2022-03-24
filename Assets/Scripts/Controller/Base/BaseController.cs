using UnityEngine;

// BaseController가 MonoBehaviour를 직접 상속할시 sealed처리가 불가능 하기 때문에 준비한 래퍼클래스
public abstract class BaseControllerBehaviorRapper : MonoBehaviour
{
    protected abstract void Awake();
}

public abstract class BaseController : BaseControllerBehaviorRapper
{
    protected static ModelInfoHolder modelInfoHolder = new ModelInfoHolder();

    // note : 객체 초기화 순서
    // Awake의 MasterDataHolder에서 데이터베이스 초기화 까지 진행
    // 이후 컨트롤러에서 model, view 등을 초기화.
    // 데이터베이스가 초기화가 된 이후에 모델을 생성, 초기화해야 하기 때문에 Controller에서는 초기화에서 Start만 사용.
    // 따라서 Awake를 봉인하고 있음
    protected override sealed void Awake()
    { }
    // model을 controller에 추가하고 싶을 시엔 modelInfoHolder.AddModel(out)으로 추가.
    protected abstract void Start();
}
