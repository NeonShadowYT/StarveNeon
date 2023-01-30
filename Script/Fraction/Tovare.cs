using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tovare : MonoBehaviour
{
    [Space]
    [Header("Скрипты")]
    public FractionManager fractionManager;
    public InventoryManager inventoryManagers;

    [Space]
    [Header("Информация")]
    public ItemScriptableObject item;
    private MoneySlot currentMoneySlot;

    public int price;

    public Image iconItem;
    public TMP_Text priceText;
    public TMP_Text itemText;

    public Button ButtonTovare;

    public void UpdateTovare(MoneySlot moneySlot)
    {
        currentMoneySlot = moneySlot;
        iconItem.sprite = item.icon;
        priceText.text = price.ToString();
        itemText.text = item.itemName;
        if (currentMoneySlot.amount >= price) ButtonTovare.interactable = true; else ButtonTovare.interactable = false;
    }
    public void Buy()
    {
        if (currentMoneySlot.amount >= price)
        {
            currentMoneySlot.amount -= price;
            inventoryManagers.AddItem(item, 1);
        }
        UpdateTovare(currentMoneySlot);
        fractionManager.FractionFunc();
    }
}
