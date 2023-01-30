using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Предмет и его количество")]
    public ItemScriptableObject item;
    public int amount;

    [Space]
    public TMP_Text itemAmountText;

    [Space]
    [Header("Иконка и прочность")]
    public Image iconGO;
    public Image _durabilityBar;

    [Space]
    public float itemDurability;

    [Space]
    [Header("Информация о слоте")]
    public ClothType clothType = ClothType.None;

    [Space]
    public bool isEmpty = true;

    public void SetIcon(Sprite icon)
    {
        iconGO.color = new Color(1, 1, 1, 1);
        iconGO.sprite = icon;
    }

    public void UpdateDurabilityBar() => _durabilityBar.fillAmount = itemDurability / 100;
}
