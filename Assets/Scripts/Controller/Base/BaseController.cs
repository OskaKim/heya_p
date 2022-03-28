using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected static ModelInfoHolder modelInfoHolder = new ModelInfoHolder();

    // model을 controller에 추가하고 싶을 시엔 modelInfoHolder.AddModel(out)으로 추가.
}
