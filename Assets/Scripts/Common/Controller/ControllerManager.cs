using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common
{
    public class ControllerManager : common.Singleton<MonoBehaviour>
    {
        #region

        // 컨트롤러를 추가할 시 ControllerType과 manageableControllers에 새로운 컨트롤러에 대한 정의를 추가할 필요가 있다.
        public enum ControllerType
        {
            Character,
            Furniture,
            FurnitureScroillView,
            GridInstall,
            TimeInfo
        };

        // 관리가능한 컨트롤러 리스트.
        private Dictionary<ControllerType, Type> manageableControllers = new Dictionary<ControllerType, Type>(){
            {ControllerType.Character, typeof(CharacterController)},
            {ControllerType.Furniture, typeof(FurnitureController)},
            {ControllerType.FurnitureScroillView, typeof(FurnitureScrollViewController)},
            {ControllerType.GridInstall, typeof(grid.GridInstallController)},
            {ControllerType.TimeInfo, typeof(timeinfo.TimeInfoController)},
        };

        #endregion

        // 관리중인 컨트롤러 리스트
        // RunController시에 add되고, StopController시에 remove됨
        private List<BaseController> managedControllers = new List<BaseController>();

        private const string controllerObjectNameFormat = "{0}Controller";

        protected sealed override void Awake()
        {
        }

        public void RunController(ControllerType controllerType)
        {
            // 해당 타입의 컨트롤러용 오브젝트를 생성
            var createdControllerObject = new GameObject(string.Format(controllerObjectNameFormat, controllerType.ToString()));
            createdControllerObject.transform.parent = transform;
            Type controllerClassType;
            if (manageableControllers.TryGetValue(controllerType, out controllerClassType))
            {
                managedControllers.Add(createdControllerObject.AddComponent(controllerClassType) as BaseController);
            }
            else
            {
                Debug.LogError($"정의 되지 않은 컨트롤러{controllerType.ToString()}");
                DestroyImmediate(createdControllerObject);
            }
        }

        public void PauseController(ControllerType controllerType)
        {
            // todo : controller일시정지
            // 현재 컨트롤러에서 update()로 업데이트 처리를 하고 있는게 아니기 때문에 어떻게 구현할지, 또 어떤 상황에 필요할지 잘 모르겠음
        }

        public void StopController(ControllerType controllerType)
        {

            var createdControllerObject = new GameObject(string.Format(controllerObjectNameFormat, controllerType.ToString()));
            var targetControllerObjectName = string.Format(controllerObjectNameFormat, controllerType.ToString());
            var targetController = managedControllers
            .FirstOrDefault(controller => controller.name == targetControllerObjectName);

            if (!targetController)
            {
                Debug.LogError($"생성되지 않은 컨트롤러{targetControllerObjectName}");
                return;
            }

            managedControllers.Remove(targetController);
            Destroy(targetController);
        }
    }
}