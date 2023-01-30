using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneySlot : MonoBehaviour
{
    public MoneyScriptableObject money;
    private int _amount;
    public int amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            moneyText.text = _amount.ToString();
        }
    }
    public TMP_Text moneyText;
}
