using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFurnitureStatusView : MonoBehaviour
{
    [SerializeField] private GameObject uiStatusTopObject;
    public event Action OnClickRotateButton;
    private void Awake() {
        Hide();    
    }

    public void Show(Vector3 position)
    {
        var screenPoint = Camera.main.WorldToScreenPoint(position);

        uiStatusTopObject.transform.position = screenPoint;
        uiStatusTopObject.SetActive(true);
    }

    public void Hide()
    {
        uiStatusTopObject.SetActive(false);
    }

    public void ClickRotateButton()
    {
        OnClickRotateButton?.Invoke();
    }
}
