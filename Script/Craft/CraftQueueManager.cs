using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CraftQueueManager : MonoBehaviour // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты")]
    public InventoryManager inventoryManager;
    public QuickslotInventory quickslotInventory;
    public CraftManager craftManager;

    [Space]
    [Header("Текущий крафт")]
    public CraftScriptableObject currentCraftItem;

    [Space]
    [Header("Информация")]
    public GameObject craftQueuePrefab;
    public TMP_InputField craftAmountInputField;  //public Button addButton; //public Button removeButton;
    public int craftTime;

    [Space]
    [Header("Одежда")]
    public InventorySlot armorSlot;
    public List<ClothAdder> clothAdders;

    public void RemoveButtonFunction()
    {
        if (int.Parse(craftAmountInputField.text) <= 1) return;
        int newAmount = int.Parse(craftAmountInputField.text) - 1;
        craftAmountInputField.text = newAmount.ToString();
    }

    public void AddButtonFunction()
    {
        if (int.Parse(craftAmountInputField.text) >= 9) return;
        int newAmount = int.Parse(craftAmountInputField.text) + 1;
        craftAmountInputField.text = newAmount.ToString();
    }

    public void AddToCraftQueue()
    {
        foreach (CraftResource craftResource in currentCraftItem.craftResources)
        {
            int amountToRemove = craftResource.craftObjectAmount * int.Parse(craftAmountInputField.text);
            foreach (InventorySlot slot in inventoryManager.slots)
            {
                if (amountToRemove <= 0) continue;
                if(slot.item == craftResource.craftObject)
                {
                    if(amountToRemove > slot.amount)
                    {
                        amountToRemove -= slot.amount;
                        slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                    }
                    else
                    {
                        slot.amount -= amountToRemove;
                        amountToRemove = 0;
                        if(slot.amount <= 0) slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData(); else slot.itemAmountText.text = slot.amount.ToString();
                    }
                }
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<CraftQueueItemDetails>().currentCraftItem == currentCraftItem)
            {
                CraftQueueItemDetails сraftQueueItemDetails = transform.GetChild(i).GetComponent<CraftQueueItemDetails>();
                сraftQueueItemDetails.craftAmount += int.Parse(craftAmountInputField.text);
                сraftQueueItemDetails.amountText.text = "X" + сraftQueueItemDetails.craftAmount.ToString();

                craftManager.currentCraftItem.FillItemDetails();
                return;
            }
        }

        CraftQueueItemDetails craftQueueItemDetails = Instantiate(craftQueuePrefab, transform).GetComponent<CraftQueueItemDetails>();

        craftQueueItemDetails.inventoryManager = inventoryManager;
        craftQueueItemDetails.craftManager = craftManager;

        craftQueueItemDetails.itemImage.sprite = currentCraftItem.finalCraft.icon;
        craftQueueItemDetails.amountText.text = craftAmountInputField.text;
        craftQueueItemDetails.craftAmount = int.Parse(craftAmountInputField.text);

        craftTime = currentCraftItem.craftTime;
        int minutes = Mathf.FloorToInt(craftTime / 60);
        int seconds = craftTime - minutes * 60;

        craftQueueItemDetails.timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        craftQueueItemDetails.craftTime = craftTime;
        craftQueueItemDetails.currentCraftItem = currentCraftItem;

        craftManager.currentCraftItem.FillItemDetails();
        quickslotInventory.CheckItemInHand();

        if (armorSlot.item == null) foreach (ClothAdder clothAdder in clothAdders) clothAdder.RemoveClothes(clothAdder.OldclothObj);
    }
}
