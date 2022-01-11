using UnityEngine;
using UnityEngine.UI;

public class UIInstallFurnitureView : MonoBehaviour
{
    [SerializeField] Transform ScrollViewContentRoot;
    [SerializeField] Transform ScrollViewContentPrefab;
    private void Start()
    {
        foreach (var resourceSprite in Resources.LoadAll<Sprite>("Furniture"))
        {
            var content = Transform.Instantiate(ScrollViewContentPrefab, ScrollViewContentRoot);
            var contentImageComponent = content.GetComponent<Image>();
            contentImageComponent.sprite = resourceSprite;
            var size = resourceSprite.bounds.size;
            var ratio = size.x / size.y;
            var contentAspectRatioFitter = content.GetComponent<AspectRatioFitter>();
            contentAspectRatioFitter.aspectRatio = ratio;
        }
    }
}
