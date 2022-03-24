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
            // controller에 필요한 view와 model이 없으면 생성하는 controllerCreator?같은걸 만들어야 할듯
            // 각 view는 특정 식별자로 생성이 될 수 있어야 하므로, 모든 view는 기본적으로 prefab임을 전제로 해야함.
            // 이를 위해서는 각 view가 디폴트로 배치되어있는 기본 오브젝트에 대한 정보를 가지고 있으야 하고, view내부에 조작하는 ui가 위치해야함
            // 각 view가 서로 알지 못하는 상태니 별 문제는 없을듯
        }

        public void PauseController(ControllerType controllerType)
        {
            // todo : controller일시정지
            // 현재 컨트롤러에서 update()로 업데이트 처리를 하고 있는게 아니기 때문에 어떻게 구현할지, 또 어떤 상황에 필요할지 잘 모르겠음
        }

        public void StopController(ControllerType controllerType)
        {
            // todo : controller정지(삭제)
            // controller삭제시 참조하던 view와 model의 참조 갯수를 -1하고, 참조 갯수가 0이 된 view와 model은 삭제
        }
    }
}