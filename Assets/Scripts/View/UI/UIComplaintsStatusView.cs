using UnityEngine;
using UnityEngine.UI;

public class UIComplaintsStatusView : MonoBehaviour
{
    [SerializeField] Text appetiteText;
    [SerializeField] Text parchedText;
    [SerializeField] Text poisonText;

    [SerializeField] private int appetiteValue;
    [SerializeField] private int parchedValue;
    [SerializeField] private int poisonValue;

    private void Awake()
    {
        var roomUI = GameObject.Find("RoomUI");
        transform.SetParent(roomUI.transform);
    }

    public void Start()
    {
        UpdateUITexts();
    }

    public void UpdateUITexts()
    {
        appetiteText.text = appetiteValue.ToString();
        parchedText.text = parchedValue.ToString();
        poisonText.text = poisonValue.ToString();

    }
}
