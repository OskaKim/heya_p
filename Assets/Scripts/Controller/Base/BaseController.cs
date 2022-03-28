using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected static ModelInfoHolder modelInfoHolder = new ModelInfoHolder();

    // model을 controller에 추가하고 싶을 시엔 modelInfoHolder.AddModel(out)으로 추가.

    // 생성시 관련 view, model을 추가
    protected abstract void OnInitialize();

    // 삭제시 관련 view, model등을 참조 카운트를 통해 필요시 삭제하는 처리를 각 컨트롤러에 추가함
    protected abstract void OnFinalize();

    private void Awake()
    {
        OnInitialize();
    }

    private void OnDestroy()
    {
        OnFinalize();
    }
}
