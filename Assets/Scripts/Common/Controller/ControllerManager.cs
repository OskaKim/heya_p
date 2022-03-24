using UnityEngine;
using System.Collections.Generic;

namespace common
{
    public class ControllerManager : common.Singleton<MonoBehaviour>
    {
        public enum ControllerType
        {
            Character,
            Furniture,
            FurnitureScroillView,
            GridInstall,
            TimeInfo
        };
        private List<BaseController> runningControllers;
        private List<BaseController> pausedControllers;

        protected sealed override void Awake()
        {
            runningControllers = new List<BaseController>();
        }

        public void RunController(ControllerType controllerType)
        {
            // todo : controller실행
        }

        public void PauseController(ControllerType controllerType)
        {
            // todo : controller일시정지
        }

        public void StopController(ControllerType controllerType)
        {
            // todo : controller정지(삭제)
        }
    }
}