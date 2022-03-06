using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected static ModelInfoHolder modelInfoHolder = new ModelInfoHolder();
    protected abstract void SetupModels();
    protected abstract void SetupViews();

    // note : 객체 초기화 순서
    // master data holder -> 데이터베이스 초기화 -> 해당 씬의 present -> 호출 된 model -> view 순서
    private void Start()
    {
        SetupModels();
        SetupViews();
    }
}
