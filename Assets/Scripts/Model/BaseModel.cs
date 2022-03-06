using UnityEngine;

//프레젠터를 관리하는 클래스를 따로 만들어서, 프레젠터에 참조되는 모델이 없으면 삭제하는 등으로 메모리 관리 하기
public class BaseModel<T> : common.Singleton<T> where T : UnityEngine.Component { }