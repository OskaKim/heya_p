using UnityEngine;
using common;

public class SceneStarter : MonoBehaviour
{
    [SerializeField] ControllerType[] startControllers;
    private void Start()
    {
        foreach (var startController in startControllers)
        {
            ControllerManager.instance.RunController(startController);
        }
    }
}
