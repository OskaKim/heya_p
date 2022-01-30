using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIFurnitureScrollView : MonoBehaviour
{
    [SerializeField] private Transform ScrollViewContentRoot;
    [SerializeField] private Transform ScrollViewContentPrefab;

    // NOTE : 스크롤뷰 ui요소
    private class UIFurnitureScrollContent
    {
        public Sprite Sprite { get; set; }
        public int id { get; set; }
    }

    // NOTE : 외부에서 설정되는 데이터
    public struct Param
    {
        public struct FurnitureScrollData
        {
            public Sprite sprite;
            public int id;
        };

        public List<FurnitureScrollData> furnitureScrollDataList;
    };
    public IObservable<int> OnSelectFurniture { get => onSelectFurniture; }
    private Subject<int> onSelectFurniture = new Subject<int>();

    private Param param;
    private List<UIFurnitureScrollContent> furnitureScrollContentList = new List<UIFurnitureScrollContent>();

    public void StartView(Param _param)
    {
        param = _param;
        CreateScrollViewContent();
    }

    private void CreateScrollViewContent()
    {
        foreach (var furnitureScrollData in param.furnitureScrollDataList)
        {
            var furnitureScrollContent = new UIFurnitureScrollContent();
            furnitureScrollContentList.Add(furnitureScrollContent);
            furnitureScrollContent.Sprite = furnitureScrollData.sprite;
            furnitureScrollContent.id = furnitureScrollData.id;
        }

        foreach (var furnitureScrollContent in furnitureScrollContentList)
        {
            var resourceSprite = furnitureScrollContent.Sprite;
            var content = Transform.Instantiate(ScrollViewContentPrefab, ScrollViewContentRoot);
            var contentImageComponent = content.GetComponent<Image>();
            contentImageComponent.sprite = resourceSprite;
            var size = resourceSprite.bounds.size;
            var ratio = size.x / size.y;
            var contentAspectRatioFitter = content.GetComponent<AspectRatioFitter>();
            contentAspectRatioFitter.aspectRatio = ratio;

            var button = content.GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ =>
            {
                onSelectFurniture.OnNext(furnitureScrollContent.id);
            });
        }
    }
}
