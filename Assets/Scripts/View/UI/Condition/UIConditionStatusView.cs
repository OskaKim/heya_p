using UnityEngine;
using UnityEngine.UI;

public class UIConditionStatusView : MonoBehaviour
{
    [SerializeField] Text HpText;
    [SerializeField] Text MpText;

    [SerializeField] private int hp;
    [SerializeField] private int mp;

    public void Start()
    {
        UpdateConditionValues();
    }

    public void UpdateConditionValues()
    {
        HpText.text = GetConditionTextFromValue(hp);
        MpText.text = GetConditionTextFromValue(mp);

    }

    private string GetConditionTextFromValue(int value)
    {
        var sign = value > 0 ? "+" : "";
        return $"{sign}{value}";
    }
}
