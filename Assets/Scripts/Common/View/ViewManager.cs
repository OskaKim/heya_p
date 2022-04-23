using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace common
{
    public class ViewManager : common.Singleton<ViewManager>
    {
        private readonly Dictionary<Type, string> prefabPaths = new Dictionary<Type, string>(){
            {typeof(timeinfo.UITimeView), "Prefabs/Views/TimeView"},
            {typeof(UIFurnitureScrollViewView), "Prefabs/Views/FurnitureScrollView"},
            {typeof(CharacterAIStatusUIView), "Prefabs/Views/CharacterAIStatusUiView"},
            {typeof(UIComplaintsStatusView), "Prefabs/Views/UIComplaintsStatusView"},
            {typeof(UIConditionStatusView), "Prefabs/Views/UIConditionStatusView"},
            {typeof(UIFurnitureStatusView), "Prefabs/Views/UIFurnitureStatusView"},
            {typeof(grid.GridTilemapView), "Prefabs/Views/GridTilemapView"},
            {typeof(CharacterView), "Prefabs/Views/Characterview"},
            {typeof(grid.GridLineDrawerView), "Prefabs/Views/GridLineDrawerView"},
            {typeof(timeinfo.UIScheduleView), "Prefabs/Views/ScheduleView"},
        };

        public T CreateViewObject<T>() where T : UnityEngine.Component
        {
            string path;
            if (!prefabPaths.TryGetValue(typeof(T), out path))
            {
                Debug.LogError($"정의된 path를 찾을 수 없습니다 : {typeof(T).ToString()}");
                return default(T);
            }
            var originObject = Resources.Load<T>(path);
            var createdObject = Instantiate<T>(originObject);
            createdObject.transform.localPosition = originObject.transform.localPosition;
            createdObject.transform.localScale = originObject.transform.localScale;
            var createdObjectRectTransform = createdObject.GetComponent<RectTransform>();
            if (createdObjectRectTransform)
            {
                var originRectTransform = originObject.GetComponent<RectTransform>();
                createdObjectRectTransform.anchoredPosition = originRectTransform.anchoredPosition;
                createdObjectRectTransform.offsetMax = originRectTransform.offsetMax;
                createdObjectRectTransform.offsetMin = originRectTransform.offsetMin;
            }
            return createdObject;
        }
    }
}