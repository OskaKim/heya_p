using UnityEngine;

public abstract class BasePresent : MonoBehaviour
{
    protected abstract void SetupModels();
    protected abstract void InitializeControllers();
    protected abstract void SetupViews();

    // note : 객체 초기화 순서
    // master data holder -> 데이터베이스 초기화 -> 해당 씬의 present -> present 내의 model, controller, view 순서
    private void Start()
    {
        SetupModels();
        InitializeControllers();
        SetupViews();
    }
}
